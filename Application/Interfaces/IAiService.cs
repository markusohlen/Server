using Application.Models;

namespace Application.Interfaces;

public interface IAiService
{
    Task<ApiResponse<List<AiMealSuggestion>>> SuggestMealsAsync(string preferences, int count = 5);
    Task<ApiResponse<AiMealPlan>> GenerateMealPlanAsync(int days, int mealsPerDay, string? dietaryNotes = null);
    Task<ApiResponse<AiRecipeImprovement>> ImproveRecipeAsync(string recipeName, string ingredients, string instructions);
}
