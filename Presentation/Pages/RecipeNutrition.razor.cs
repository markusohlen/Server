using Microsoft.AspNetCore.Components;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Pages;

public partial class RecipeNutrition
{
    [Inject]
    private MealPlannerApiService Api { get; set; } = default!;

    private string recipeUrl = string.Empty;
    private string? errorMessage;
    private bool analyzing;
    private bool lookingUpAll;
    private ImportedRecipeData? recipe;
    private List<IngredientNutritionRow> ingredientRows = [];
    private IngredientNutritionRow totalRow = new();

    /// <summary>
    /// Hardcoded nutrition data for Steve's Lava Chicken per 100 g.
    /// </summary>
    private static readonly IngredientNutritionData LavaChickenNutrition = new()
    {
        CaloriesPer100g = 190,
        ProteinPer100g = 25.0,
        FatPer100g = 9.0,
        SaturatedFatPer100g = 2.5,
        CarbsPer100g = 3.0,
        SugarPer100g = 1.0,
        FiberPer100g = 0.5,
        SodiumPer100g = 0.65,
        IronPer100g = 1.2,
        CalciumPer100g = 20.0,
        VitaminAPer100g = 50.0,
        VitaminCPer100g = 2.0
    };

    private async Task AnalyzeRecipe()
    {
        if (string.IsNullOrWhiteSpace(recipeUrl))
            return;

        analyzing = true;
        errorMessage = null;
        recipe = null;
        ingredientRows = [];

        try
        {
            var data = await Api.ImportRecipeFromUrlAsync(recipeUrl);
            if (data is null)
            {
                errorMessage = "Could not extract recipe from the provided URL.";
                return;
            }

            recipe = data;
            lookingUpAll = true;
            StateHasChanged();

            var rows = new List<IngredientNutritionRow>();

            foreach (var ingredient in recipe.Ingredients)
            {
                var weightGrams = EstimateWeightGrams(ingredient.Volume, ingredient.Measure);
                var nutrition = await Api.LookupIngredientNutritionAsync(ingredient.Name);

                rows.Add(BuildRow(ingredient.Name, weightGrams, nutrition));
            }

            // Always add 250 g of Steve's Lava Chicken
            rows.Add(BuildRow("Steve's Lava Chicken", 250, LavaChickenNutrition, isLavaChicken: true));

            // Compute totals
            totalRow = new IngredientNutritionRow
            {
                Name = "TOTAL",
                IsTotal = true,
                WeightGrams = rows.Sum(r => r.WeightGrams),
                Calories = rows.Sum(r => r.Calories),
                Protein = rows.Sum(r => r.Protein),
                Fat = rows.Sum(r => r.Fat),
                SaturatedFat = rows.Sum(r => r.SaturatedFat),
                Carbs = rows.Sum(r => r.Carbs),
                Sugar = rows.Sum(r => r.Sugar),
                Fiber = rows.Sum(r => r.Fiber),
                Sodium = rows.Sum(r => r.Sodium),
                Iron = rows.Sum(r => r.Iron),
                Calcium = rows.Sum(r => r.Calcium),
                VitaminA = rows.Sum(r => r.VitaminA),
                VitaminC = rows.Sum(r => r.VitaminC)
            };

            rows.Add(totalRow);
            ingredientRows = rows;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error analyzing recipe: {ex.Message}");
            errorMessage = "Failed to analyze the recipe. Please try again.";
        }
        finally
        {
            analyzing = false;
            lookingUpAll = false;
        }
    }

    private static IngredientNutritionRow BuildRow(string name, double weightGrams, IngredientNutritionData? nutrition, bool isLavaChicken = false)
    {
        var factor = weightGrams / 100.0;
        return new IngredientNutritionRow
        {
            Name = name,
            IsLavaChicken = isLavaChicken,
            WeightGrams = weightGrams,
            Calories = (nutrition?.CaloriesPer100g ?? 0) * factor,
            Protein = (nutrition?.ProteinPer100g ?? 0) * factor,
            Fat = (nutrition?.FatPer100g ?? 0) * factor,
            SaturatedFat = (nutrition?.SaturatedFatPer100g ?? 0) * factor,
            Carbs = (nutrition?.CarbsPer100g ?? 0) * factor,
            Sugar = (nutrition?.SugarPer100g ?? 0) * factor,
            Fiber = (nutrition?.FiberPer100g ?? 0) * factor,
            Sodium = (nutrition?.SodiumPer100g ?? 0) * factor,
            Iron = (nutrition?.IronPer100g ?? 0) * factor,
            Calcium = (nutrition?.CalciumPer100g ?? 0) * factor,
            VitaminA = (nutrition?.VitaminAPer100g ?? 0) * factor,
            VitaminC = (nutrition?.VitaminCPer100g ?? 0) * factor
        };
    }

    /// <summary>
    /// Convert volume + measure code into an approximate weight in grams.
    /// </summary>
    private static double EstimateWeightGrams(int volume, int measure) => measure switch
    {
        0 => volume,             // Gram
        1 => volume * 100.0,     // Hektogram
        2 => volume * 1000.0,    // Kilogram
        10 => volume,            // Milliliter (≈ 1 g)
        11 => volume * 100.0,    // Deciliter
        12 => volume * 1000.0,   // Liter
        20 => volume * 240.0,    // Kopp (cup)
        21 => volume * 5.0,      // Tesked (teaspoon)
        22 => volume * 15.0,     // Matsked (tablespoon)
        30 => volume * 28.35,    // Uns (ounce)
        31 => volume * 453.6,    // Pund (pound)
        40 => volume * 100.0,    // Styck (piece, rough estimate)
        41 => volume * 30.0,     // Skiva (slice)
        42 => volume * 40.0,     // Klyfta (wedge)
        _ => volume
    };

    public class IngredientNutritionRow
    {
        public string Name { get; set; } = string.Empty;
        public bool IsLavaChicken { get; set; }
        public bool IsTotal { get; set; }
        public double WeightGrams { get; set; }
        public double Calories { get; set; }
        public double Protein { get; set; }
        public double Fat { get; set; }
        public double SaturatedFat { get; set; }
        public double Carbs { get; set; }
        public double Sugar { get; set; }
        public double Fiber { get; set; }
        public double Sodium { get; set; }
        public double Iron { get; set; }
        public double Calcium { get; set; }
        public double VitaminA { get; set; }
        public double VitaminC { get; set; }
    }
}
