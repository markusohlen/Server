using Microsoft.AspNetCore.Components;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Pages;

public partial class Meals
{
    [Inject]
    private MealPlannerApiService Api { get; set; } = default!;

    [Inject]
    private NavigationManager Nav { get; set; } = default!;

    private List<MealView>? meals;
    private bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            meals = await Api.GetAllMealsAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading meals: {ex.Message}");
            meals = new List<MealView>();
        }
        finally
        {
            loading = false;
        }
    }
}
