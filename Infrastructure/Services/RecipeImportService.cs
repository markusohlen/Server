using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Application.Interfaces;
using Application.Models;

namespace Infrastructure.Services;

public class RecipeImportService : IRecipeImportService
{
    private readonly HttpClient _httpClient;

    public RecipeImportService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ImportedRecipeData?> ImportFromUrlAsync(string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("User-Agent", "MealPlanner/1.0");
        request.Headers.Add("Accept", "text/html");

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        var html = await response.Content.ReadAsStringAsync();
        var recipeElement = FindRecipeJsonLd(html);
        if (recipeElement == null)
            return null;

        return ParseRecipe(recipeElement.Value);
    }

    private static JsonElement? FindRecipeJsonLd(string html)
    {
        foreach (var json in ExtractJsonLdBlocks(html))
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                var found = FindRecipeElement(doc.RootElement);
                if (found != null)
                    return found.Value.Clone();
            }
            catch (JsonException)
            {
                continue;
            }
        }

        return null;
    }

    private static JsonElement? FindRecipeElement(JsonElement element)
    {
        if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                var found = FindRecipeElement(item);
                if (found != null)
                    return found;
            }
        }
        else if (element.ValueKind == JsonValueKind.Object)
        {
            if (IsRecipeType(element))
                return element;

            if (element.TryGetProperty("@graph", out var graph))
                return FindRecipeElement(graph);
        }

        return null;
    }

    private static bool IsRecipeType(JsonElement element)
    {
        if (!element.TryGetProperty("@type", out var typeProp))
            return false;

        if (typeProp.ValueKind == JsonValueKind.String)
            return typeProp.GetString()?.Equals("Recipe", StringComparison.OrdinalIgnoreCase) == true;

        if (typeProp.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in typeProp.EnumerateArray())
            {
                if (item.ValueKind == JsonValueKind.String &&
                    item.GetString()?.Equals("Recipe", StringComparison.OrdinalIgnoreCase) == true)
                    return true;
            }
        }

        return false;
    }

    private static List<string> ExtractJsonLdBlocks(string html)
    {
        var blocks = new List<string>();
        const string openTag = "<script type=\"application/ld+json\">";
        const string closeTag = "</script>";

        var pos = 0;
        while (true)
        {
            pos = html.IndexOf(openTag, pos, StringComparison.OrdinalIgnoreCase);
            if (pos == -1) break;

            var start = pos + openTag.Length;
            var end = html.IndexOf(closeTag, start, StringComparison.OrdinalIgnoreCase);
            if (end == -1) break;

            blocks.Add(html[start..end].Trim());
            pos = end + closeTag.Length;
        }

        return blocks;
    }

    private static ImportedRecipeData ParseRecipe(JsonElement recipe)
    {
        var data = new ImportedRecipeData
        {
            Name = recipe.TryGetProperty("name", out var name)
                ? name.GetString() ?? string.Empty
                : string.Empty,
            Categories = ParseCategories(recipe),
            Portions = ParsePortions(recipe),
            Ingredients = ParseIngredients(recipe),
            Instructions = ParseInstructions(recipe)
        };

        return data;
    }

    private static List<string> ParseCategories(JsonElement recipe)
    {
        if (!recipe.TryGetProperty("recipeCategory", out var cat))
            return [];

        if (cat.ValueKind == JsonValueKind.String)
            return [cat.GetString() ?? string.Empty];

        if (cat.ValueKind == JsonValueKind.Array)
            return cat.EnumerateArray()
                .Where(c => c.ValueKind == JsonValueKind.String)
                .Select(c => c.GetString() ?? string.Empty)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .ToList();

        return [];
    }

    private static int ParsePortions(JsonElement recipe)
    {
        if (!recipe.TryGetProperty("recipeYield", out var yield))
            return 1;

        var text = yield.ValueKind switch
        {
            JsonValueKind.String => yield.GetString(),
            JsonValueKind.Number => yield.GetInt32().ToString(),
            JsonValueKind.Array => yield.EnumerateArray().FirstOrDefault().GetString(),
            _ => null
        };

        if (text != null)
        {
            var digits = new string(text.TakeWhile(char.IsDigit).ToArray());
            if (int.TryParse(digits, out var portions) && portions > 0)
                return portions;
        }

        return 1;
    }

    private static List<ImportedIngredient> ParseIngredients(JsonElement recipe)
    {
        if (!recipe.TryGetProperty("recipeIngredient", out var ingredients))
            return [];

        if (ingredients.ValueKind != JsonValueKind.Array)
            return [];

        return ingredients.EnumerateArray()
            .Where(i => i.ValueKind == JsonValueKind.String)
            .Select(i => i.GetString()?.Trim() ?? string.Empty)
            .Where(i => !string.IsNullOrWhiteSpace(i))
            .Select(ParseIngredientText)
            .ToList();
    }

    private static readonly Dictionary<string, int> MeasureMap = new(StringComparer.OrdinalIgnoreCase)
    {
        // Swedish
        ["g"] = 0, ["gram"] = 0,
        ["hg"] = 1, ["hekto"] = 1, ["hektogram"] = 1,
        ["kg"] = 2, ["kilo"] = 2, ["kilogram"] = 2,
        ["ml"] = 10, ["milliliter"] = 10,
        ["cl"] = 11, ["dl"] = 11, ["deciliter"] = 11,
        ["l"] = 12, ["liter"] = 12,
        ["kopp"] = 20, ["koppar"] = 20,
        ["tsk"] = 21, ["tesked"] = 21, ["teskedar"] = 21,
        ["msk"] = 22, ["matsked"] = 22, ["matskedar"] = 22,
        ["uns"] = 30,
        ["pund"] = 31,
        ["st"] = 40, ["styck"] = 40, ["stycken"] = 40,
        ["skiva"] = 41, ["skivor"] = 41,
        ["klyfta"] = 42, ["klyftor"] = 42,
        // English
        ["grams"] = 0,
        ["hectogram"] = 1, ["hectograms"] = 1,
        ["kilograms"] = 2,
        ["milliliters"] = 10,
        ["deciliters"] = 11,
        ["liters"] = 12,
        ["cup"] = 20, ["cups"] = 20,
        ["tsp"] = 21, ["teaspoon"] = 21, ["teaspoons"] = 21,
        ["tbsp"] = 22, ["tablespoon"] = 22, ["tablespoons"] = 22,
        ["oz"] = 30, ["ounce"] = 30, ["ounces"] = 30,
        ["lb"] = 31, ["lbs"] = 31, ["pound"] = 31, ["pounds"] = 31,
        ["piece"] = 40, ["pieces"] = 40, ["pcs"] = 40,
        ["slice"] = 41, ["slices"] = 41,
        ["clove"] = 42, ["cloves"] = 42,
    };

    private static ImportedIngredient ParseIngredientText(string text)
    {
        var remaining = text.Trim();

        var (quantity, afterQuantity) = ExtractQuantity(remaining);
        remaining = afterQuantity.TrimStart();

        int measure;
        if (quantity > 0)
        {
            var (parsedMeasure, afterMeasure) = ExtractMeasure(remaining);
            if (parsedMeasure.HasValue)
            {
                measure = parsedMeasure.Value;
                remaining = afterMeasure.TrimStart();
            }
            else
            {
                measure = 40; // Piece
            }
        }
        else
        {
            measure = 0; // Gram
        }

        var name = remaining.Trim();
        if (string.IsNullOrWhiteSpace(name))
            name = text.Trim();

        return new ImportedIngredient
        {
            Name = name,
            Volume = quantity > 0 ? Math.Max(1, (int)Math.Round(quantity)) : 0,
            Measure = measure
        };
    }

    private static (double quantity, string remaining) ExtractQuantity(string text)
    {
        var s = text;

        // Skip common approximation prefixes
        var prefixMatch = Regex.Match(s, @"^(ca\.?|cirka|ungefär|about|approximately)\s+", RegexOptions.IgnoreCase);
        if (prefixMatch.Success)
            s = s[prefixMatch.Length..];

        // Mixed fraction: "1 1/2"
        var match = Regex.Match(s, @"^(\d+)\s+(\d+)\s*/\s*(\d+)");
        if (match.Success)
        {
            var whole = int.Parse(match.Groups[1].Value);
            var num = int.Parse(match.Groups[2].Value);
            var den = int.Parse(match.Groups[3].Value);
            var total = den > 0 ? whole + (double)num / den : whole;
            return (total, s[match.Length..]);
        }

        // Simple fraction: "1/2"
        match = Regex.Match(s, @"^(\d+)\s*/\s*(\d+)");
        if (match.Success)
        {
            var num = int.Parse(match.Groups[1].Value);
            var den = int.Parse(match.Groups[2].Value);
            var total = den > 0 ? (double)num / den : 0;
            return (total, s[match.Length..]);
        }

        // Decimal: "1.5" or "1,5"
        double value = 0;
        match = Regex.Match(s, @"^(\d+[\.,]\d+)");
        if (match.Success)
        {
            value = double.Parse(match.Groups[1].Value.Replace(',', '.'), CultureInfo.InvariantCulture);
            s = s[match.Length..];
        }
        else
        {
            // Integer: "200"
            match = Regex.Match(s, @"^(\d+)");
            if (match.Success)
            {
                value = int.Parse(match.Groups[1].Value);
                s = s[match.Length..];
            }
        }

        // Trailing unicode fraction: "1½"
        if (s.Length > 0)
        {
            var (frac, len) = GetUnicodeFraction(s[0]);
            if (len > 0)
            {
                value += frac;
                s = s[len..];
            }
        }

        // Standalone unicode fraction at the start (no number before it)
        if (value == 0 && text.Length > 0)
        {
            var (frac, len) = GetUnicodeFraction(text[0]);
            if (len > 0)
                return (frac, text[len..]);
        }

        return (value, s);
    }

    private static (double value, int length) GetUnicodeFraction(char c) => c switch
    {
        '½' => (0.5, 1),
        '⅓' => (1.0 / 3, 1),
        '⅔' => (2.0 / 3, 1),
        '¼' => (0.25, 1),
        '¾' => (0.75, 1),
        '⅕' => (0.2, 1),
        '⅖' => (0.4, 1),
        '⅗' => (0.6, 1),
        '⅘' => (0.8, 1),
        '⅙' => (1.0 / 6, 1),
        '⅚' => (5.0 / 6, 1),
        '⅛' => (0.125, 1),
        '⅜' => (0.375, 1),
        '⅝' => (0.625, 1),
        '⅞' => (0.875, 1),
        _ => (0, 0)
    };

    private static (int? measure, string remaining) ExtractMeasure(string text)
    {
        var s = text.TrimStart();
        var match = Regex.Match(s, @"^(\S+)");
        if (match.Success)
        {
            var word = match.Groups[1].Value.TrimEnd('.', ',', ';');
            if (MeasureMap.TryGetValue(word, out var measure))
                return (measure, s[match.Length..]);
        }

        return (null, s);
    }

    private static List<string> ParseInstructions(JsonElement recipe)
    {
        if (!recipe.TryGetProperty("recipeInstructions", out var instructions))
            return [];

        if (instructions.ValueKind == JsonValueKind.String)
            return [instructions.GetString()?.Trim() ?? string.Empty];

        if (instructions.ValueKind != JsonValueKind.Array)
            return [];

        var steps = new List<string>();
        foreach (var item in instructions.EnumerateArray())
        {
            if (item.ValueKind == JsonValueKind.String)
            {
                var text = item.GetString()?.Trim();
                if (!string.IsNullOrWhiteSpace(text))
                    steps.Add(text);
            }
            else if (item.ValueKind == JsonValueKind.Object)
            {
                if (item.TryGetProperty("text", out var text))
                {
                    var stepText = text.GetString()?.Trim();
                    if (!string.IsNullOrWhiteSpace(stepText))
                        steps.Add(stepText);
                }
                else if (item.TryGetProperty("itemListElement", out var subSteps) &&
                         subSteps.ValueKind == JsonValueKind.Array)
                {
                    foreach (var sub in subSteps.EnumerateArray())
                    {
                        if (sub.TryGetProperty("text", out var subText))
                        {
                            var s = subText.GetString()?.Trim();
                            if (!string.IsNullOrWhiteSpace(s))
                                steps.Add(s);
                        }
                    }
                }
            }
        }

        return steps;
    }
}
