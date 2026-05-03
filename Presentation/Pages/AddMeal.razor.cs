using Microsoft.AspNetCore.Components;
using Presentation.Models;
using Presentation.Services;

namespace Presentation.Pages;

public partial class AddMeal
{
    [Inject]
    private MealPlannerApiService Api { get; set; } = default!;

    [Inject]
    private NavigationManager Nav { get; set; } = default!;

    private CreateMealModel model = new();
    private bool saving;
    private string? errorMessage;
    private List<string> ingredientNames = [];
    private string importUrl = string.Empty;
    private bool importing;

    private readonly List<MeasureOption> measureOptions =
    [
        new(0, "Gram"),
        new(1, "Hektogram"),
        new(2, "Kilogram"),
        new(10, "Milliliter"),
        new(11, "Deciliter"),
        new(12, "Liter"),
        new(20, "Kopp"),
        new(21, "Tesked"),
        new(22, "Matsked"),
        new(30, "Uns"),
        new(31, "Pund"),
        new(40, "Styck"),
        new(41, "Skiva"),
        new(42, "Klyfta")
    ];

    private void AddIngredient() => model.Ingredients.Add(new());

    private void RemoveIngredient(int index)
    {
        if (model.Ingredients.Count > 1)
            model.Ingredients.RemoveAt(index);
    }

    private void AddInstruction() => model.Instructions.Add(new());

    private void RemoveInstruction(int index)
    {
        if (model.Instructions.Count > 1)
            model.Instructions.RemoveAt(index);
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ingredientNames = await Api.GetAllIngredientNamesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading ingredient names: {ex.Message}");
        }
    }

    private async Task ImportFromUrl()
    {
        if (string.IsNullOrWhiteSpace(importUrl)) return;

        importing = true;
        errorMessage = null;
        try
        {
            var data = await Api.ImportRecipeFromUrlAsync(importUrl);
            if (data != null)
            {
                model.Name = data.Name;
                model.Categories = string.Join(", ", data.Categories);
                model.Portions = data.Portions > 0 ? data.Portions : 1;
                model.Ingredients = data.Ingredients
                    .Select(i => new CreateIngredientModel
                    {
                        Name = i.Name,
                        Volume = i.Volume,
                        Measure = i.Measure
                    })
                    .ToList();
                if (model.Ingredients.Count == 0)
                    model.Ingredients.Add(new());
                model.Instructions = data.Instructions
                    .Select(text => new CreateInstructionStepModel { Text = text })
                    .ToList();
                if (model.Instructions.Count == 0)
                    model.Instructions.Add(new());
            }
            else
            {
                errorMessage = "Could not extract recipe from the provided URL.";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing recipe: {ex.Message}");
            errorMessage = "Failed to import recipe from URL.";
        }
        finally
        {
            importing = false;
        }
    }

    private async Task LookupNutrition(int index)
    {
        var ingredient = model.Ingredients[index];
        if (string.IsNullOrWhiteSpace(ingredient.Name)) return;

        ingredient.LookingUp = true;
        try
        {
            var data = await Api.LookupIngredientNutritionAsync(ingredient.Name);
            if (data != null)
            {
                ingredient.Calories = data.CaloriesPer100g;
                ingredient.Protein = (int)data.ProteinPer100g;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error looking up nutrition: {ex.Message}");
        }
        finally
        {
            ingredient.LookingUp = false;
        }
    }

    private async Task OnSubmit()
    {
        saving = true;
        errorMessage = null;
        try
        {
            await Api.AddMealAsync(model);
            Nav.NavigateTo("meals");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding meal: {ex.Message}");
            errorMessage = "Failed to save the recipe. Please try again.";
        }
        finally
        {
            saving = false;
        }
    }
}
