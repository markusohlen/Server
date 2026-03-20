using System.Text.Json;
using Application.Interfaces;
using Application.Models;

namespace Infrastructure.Services;

public class IngredientLookupService : IIngredientLookupService
{
    private readonly HttpClient _httpClient;

    public IngredientLookupService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IngredientNutritionData?> LookupNutritionAsync(string ingredientName)
    {
        var encodedName = Uri.EscapeDataString(ingredientName);
        var url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={encodedName}&search_simple=1&action=process&json=1&page_size=1";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return null;

        using var doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
        var root = doc.RootElement;

        if (!root.TryGetProperty("products", out var products) || products.GetArrayLength() == 0)
            return null;

        var product = products[0];
        if (!product.TryGetProperty("nutriments", out var nutriments))
            return null;

        var calories = 0;
        var protein = 0;

        if (nutriments.TryGetProperty("energy-kcal_100g", out var kcal))
            calories = (int)Math.Round(kcal.GetDouble());

        if (nutriments.TryGetProperty("proteins_100g", out var prot))
            protein = (int)Math.Round(prot.GetDouble());

        return new IngredientNutritionData
        {
            CaloriesPer100g = calories,
            ProteinPer100g = protein
        };
    }
}
