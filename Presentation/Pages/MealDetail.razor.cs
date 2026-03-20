using Microsoft.AspNetCore.Components;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Pages;

public partial class MealDetail
{
    [Inject]
    private MealPlannerApiService Api { get; set; } = default!;

    [Inject]
    private NavigationManager Nav { get; set; } = default!;

    [Parameter]
    public Guid Id { get; set; }

    private MealView? meal;
    private bool loading = true;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            meal = await Api.GetMealByIdAsync(Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading meal: {ex.Message}");
        }
        finally
        {
            loading = false;
        }
    }
}
