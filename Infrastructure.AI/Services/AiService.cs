using System.Text.Json;
using Application.AI.Prompts;
using Application.Interfaces;
using Application.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

namespace Infrastructure.AI.Services;

public class AiService : IAiService
{
    private readonly IChatClient _chatClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public AiService(IChatClient chatClient)
    {
        _chatClient = chatClient;
    }

    public async Task<ApiResponse<List<AiMealSuggestion>>> SuggestMealsAsync(string preferences, int count = 5)
    {
        var json = await SendChatAsync(
            MealPrompts.SuggestMealsSystem,
            MealPrompts.SuggestMeals(preferences, count));

        if (json is null)
            return ApiResponse<List<AiMealSuggestion>>.Fail("Failed to get a response from the AI service.");

        var suggestions = JsonSerializer.Deserialize<List<AiMealSuggestion>>(json, JsonOptions);
        return suggestions is not null
            ? ApiResponse<List<AiMealSuggestion>>.Ok(suggestions)
            : ApiResponse<List<AiMealSuggestion>>.Fail("Failed to parse AI response.");
    }

    public async Task<ApiResponse<AiMealPlan>> GenerateMealPlanAsync(int days, int mealsPerDay, string? dietaryNotes = null)
    {
        var json = await SendChatAsync(
            MealPrompts.GenerateMealPlanSystem,
            MealPrompts.GenerateMealPlan(days, mealsPerDay, dietaryNotes));

        if (json is null)
            return ApiResponse<AiMealPlan>.Fail("Failed to get a response from the AI service.");

        var plan = JsonSerializer.Deserialize<AiMealPlan>(json, JsonOptions);
        return plan is not null
            ? ApiResponse<AiMealPlan>.Ok(plan)
            : ApiResponse<AiMealPlan>.Fail("Failed to parse AI response.");
    }

    public async Task<ApiResponse<AiRecipeImprovement>> ImproveRecipeAsync(string recipeName, string ingredients, string instructions)
    {
        var json = await SendChatAsync(
            MealPrompts.ImproveRecipeSystem,
            MealPrompts.ImproveRecipe(recipeName, ingredients, instructions));

        if (json is null)
            return ApiResponse<AiRecipeImprovement>.Fail("Failed to get a response from the AI service.");

        var improvement = JsonSerializer.Deserialize<AiRecipeImprovement>(json, JsonOptions);
        return improvement is not null
            ? ApiResponse<AiRecipeImprovement>.Ok(improvement)
            : ApiResponse<AiRecipeImprovement>.Fail("Failed to parse AI response.");
    }

    private async Task<string?> SendChatAsync(string systemPrompt, string userPrompt)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, systemPrompt),
            new(ChatRole.User, userPrompt)
        };

        var response = await _chatClient.GetResponseAsync(messages);
        return response.Text;
    }
}
