using System.Net.Http.Json;
using Presentation.Models;

namespace Presentation.Services;

public class MealPlannerApiService
{
    private readonly HttpClient _http;

    public MealPlannerApiService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Guid>> GetAllMealIdsAsync()
    {
        var result = await _http.GetFromJsonAsync<List<Guid>>("mealplanner/getallmealids");
        return result ?? new List<Guid>();
    }

    public async Task<List<MealView>> GetAllMealsAsync()
    {
        var result = await _http.GetFromJsonAsync<List<MealView>>("mealplanner/getallmeals");
        return result ?? new List<MealView>();
    }

    public async Task<MealView?> GetMealByIdAsync(Guid id)
    {
        return await _http.GetFromJsonAsync<MealView>($"mealplanner/getmealbyid/{id}");
    }

    public async Task<List<string>> GetAllIngredientNamesAsync()
    {
        var result = await _http.GetFromJsonAsync<List<string>>("mealplanner/getallingredientnames");
        return result ?? [];
    }

    public async Task<IngredientNutritionData?> LookupIngredientNutritionAsync(string name)
    {
        var encodedName = Uri.EscapeDataString(name);
        try
        {
            return await _http.GetFromJsonAsync<IngredientNutritionData>($"mealplanner/lookupingredient/{encodedName}");
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    public async Task<ImportedRecipeData?> ImportRecipeFromUrlAsync(string url)
    {
        var response = await _http.PostAsJsonAsync("mealplanner/importrecipe", new { url });
        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<ImportedRecipeData>();
    }

    public async Task AddMealAsync(CreateMealModel model)
    {
        var categories = string.IsNullOrWhiteSpace(model.Categories)
            ? new List<string>()
            : model.Categories
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

        var payload = new
        {
            name = model.Name,
            categories,
            portions = model.Portions,
            ingredients = model.Ingredients.Select(i => new
            {
                name = i.Name,
                calories = i.Calories ?? 0,
                protein = i.Protein ?? 0,
                price = i.Price ?? 0m,
                volume = i.Volume,
                measure = i.Measure
            }),
            instructions = model.Instructions.Select((s, idx) => new
            {
                stepNumber = idx + 1,
                text = s.Text,
                instructionIngredients = Array.Empty<object>()
            })
        };

        var response = await _http.PostAsJsonAsync("mealplanner/addmeal", payload);
        response.EnsureSuccessStatusCode();
    }
}
