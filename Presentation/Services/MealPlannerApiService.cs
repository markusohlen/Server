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
}
