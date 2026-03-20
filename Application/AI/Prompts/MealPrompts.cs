namespace Application.AI.Prompts;

public static class MealPrompts
{
    public const string SuggestMealsSystem = """
        You are a helpful meal planning assistant. You suggest meals based on user preferences and available ingredients.
        Always respond in valid JSON format matching the requested schema.
        Do not include any text outside the JSON response.
        """;

    public static string SuggestMeals(string preferences, int count) => $"""
        Suggest {count} meal ideas based on the following preferences:
        {preferences}

        Respond with a JSON array of objects with the following properties:
        - "name": string (meal name)
        - "description": string (brief description)
        - "estimatedMinutes": number (estimated cooking time in minutes)
        - "categories": string[] (meal categories like "lunch", "dinner", etc.)
        """;

    public const string GenerateMealPlanSystem = """
        You are a professional meal planner. You create balanced weekly meal plans.
        Always respond in valid JSON format matching the requested schema.
        Do not include any text outside the JSON response.
        """;

    public static string GenerateMealPlan(int days, int mealsPerDay, string? dietaryNotes) => $"""
        Create a meal plan for {days} days with {mealsPerDay} meals per day.
        {(dietaryNotes is not null ? $"Dietary notes: {dietaryNotes}" : "")}

        Respond with a JSON object with a "days" array, each containing:
        - "day": number (1-based)
        - "meals": array of objects with:
          - "type": string ("breakfast", "lunch", "dinner", or "snack")
          - "name": string (meal name)
          - "description": string (brief description)
        """;

    public const string ImproveRecipeSystem = """
        You are a creative chef assistant. You suggest improvements to recipes.
        Always respond in valid JSON format matching the requested schema.
        Do not include any text outside the JSON response.
        """;

    public static string ImproveRecipe(string recipeName, string ingredients, string instructions) => $"""
        Suggest improvements for this recipe:

        Name: {recipeName}
        Ingredients: {ingredients}
        Instructions: {instructions}

        Respond with a JSON object with:
        - "suggestions": string[] (list of improvement suggestions)
        - "alternativeIngredients": array of objects with "original" and "alternative" string properties
        - "tips": string[] (cooking tips)
        """;
}
