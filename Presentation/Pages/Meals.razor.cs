using Microsoft.AspNetCore.Components;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Pages;

public partial class Meals : IDisposable
{
    [Inject]
    private MealPlannerApiService Api { get; set; } = default!;

    [Inject]
    private NavigationManager Nav { get; set; } = default!;

    private List<MealView>? meals;
    private bool loading = true;

    private string searchText = string.Empty;
    private Timer? debounceTimer;

    private List<MealView> FilteredMeals
    {
        get
        {
            if (meals is null) return [];

            if (string.IsNullOrWhiteSpace(searchText)) return meals;

            return meals.Where(m =>
                m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                m.Categories.Any(c => c.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }
    }

    private void OnSearchInput(ChangeEventArgs e)
    {
        var value = e.Value?.ToString() ?? string.Empty;
        debounceTimer?.Dispose();
        debounceTimer = new Timer(_ =>
        {
            InvokeAsync(() =>
            {
                searchText = value;
                StateHasChanged();
            });
        }, null, 300, Timeout.Infinite);
    }

    private void ClearSearch()
    {
        searchText = string.Empty;
        debounceTimer?.Dispose();
    }

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

    public void Dispose()
    {
        debounceTimer?.Dispose();
    }
}
