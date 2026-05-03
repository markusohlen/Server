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

        return new IngredientNutritionData
        {
            CaloriesPer100g = GetIntNutrient(nutriments, "energy-kcal_100g"),
            ProteinPer100g = GetDoubleNutrient(nutriments, "proteins_100g"),
            FatPer100g = GetDoubleNutrient(nutriments, "fat_100g"),
            SaturatedFatPer100g = GetDoubleNutrient(nutriments, "saturated-fat_100g"),
            CarbsPer100g = GetDoubleNutrient(nutriments, "carbohydrates_100g"),
            SugarPer100g = GetDoubleNutrient(nutriments, "sugars_100g"),
            FiberPer100g = GetDoubleNutrient(nutriments, "fiber_100g"),
            SodiumPer100g = GetDoubleNutrient(nutriments, "sodium_100g"),
            IronPer100g = GetDoubleNutrient(nutriments, "iron_100g"),
            CalciumPer100g = GetDoubleNutrient(nutriments, "calcium_100g"),
            VitaminAPer100g = GetDoubleNutrient(nutriments, "vitamin-a_100g"),
            VitaminCPer100g = GetDoubleNutrient(nutriments, "vitamin-c_100g")
        };
    }

    private static int GetIntNutrient(JsonElement nutriments, string key)
    {
        if (nutriments.TryGetProperty(key, out var value))
            return (int)Math.Round(value.GetDouble());
        return 0;
    }

    private static double GetDoubleNutrient(JsonElement nutriments, string key)
    {
        if (nutriments.TryGetProperty(key, out var value))
            return Math.Round(value.GetDouble(), 2);
        return 0;
    }
}
