using Application.Models;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class MealPlannerService
{
    private readonly IMealRepository _mealRepository;
    private readonly IIngredientLookupService _ingredientLookup;
    private readonly IRecipeImportService _recipeImport;

    public MealPlannerService(IMealRepository mealRepository, IIngredientLookupService ingredientLookup, IRecipeImportService recipeImport)
    {
        _mealRepository = mealRepository;
        _ingredientLookup = ingredientLookup;
        _recipeImport = recipeImport;
    }

    public async Task<ApiResponse<List<Guid>>> GetAllMealIdsAsync()
    {
        var mealIds = await _mealRepository.GetAllMealIdsAsync();
        return ApiResponse<List<Guid>>.Ok(mealIds);
    }

    public async Task<ApiResponse<List<Meal>>> GetAllMealsAsync()
    {
        var meals = await _mealRepository.GetAllMealsAsync();
        return ApiResponse<List<Meal>>.Ok(meals);
    }

    public async Task<ApiResponse<List<MealView>>> GetAllMealViewsAsync()
    {
        var meals = await _mealRepository.GetAllMealViewsAsync();
        return ApiResponse<List<MealView>>.Ok(meals);
    }

    public async Task<ApiResponse<MealView>> GetMealByIdResponseAsync(Guid id)
    {
        var meal = await _mealRepository.GetMealByIdAsync(id);
        if (meal == null)
            return ApiResponse<MealView>.Fail("Meal not found");

        return ApiResponse<MealView>.Ok(meal);
    }

    public async Task<ApiResponse<string>> AddMealAsync(Meal meal)
    {
        await _mealRepository.AddMealAsync(meal);

        return ApiResponse<string>.Ok("Meal added");
    }

    public async Task<ApiResponse<string>> UpdateMealAsync(Meal meal)
    {
        await _mealRepository.UpdateMealAsync(meal);

        return ApiResponse<string>.Ok("Meal updated");
    }

    public async Task<ApiResponse<string>> DeleteMealAsync(Guid id)
    {
        await _mealRepository.DeleteMealAsync(id);

        return ApiResponse<string>.Ok("Meal deleted");
    }

    public async Task<ApiResponse<List<string>>> GetAllIngredientNamesAsync()
    {
        var names = await _mealRepository.GetAllIngredientNamesAsync();
        return ApiResponse<List<string>>.Ok(names);
    }

    public async Task<ApiResponse<IngredientNutritionData>> LookupIngredientNutritionAsync(string name)
    {
        var data = await _ingredientLookup.LookupNutritionAsync(name);
        if (data == null)
            return ApiResponse<IngredientNutritionData>.Fail("Nutrition data not found");

        return ApiResponse<IngredientNutritionData>.Ok(data);
    }

    public async Task<ApiResponse<ImportedRecipeData>> ImportRecipeAsync(string url)
    {
        var data = await _recipeImport.ImportFromUrlAsync(url);
        if (data == null)
            return ApiResponse<ImportedRecipeData>.Fail("Could not extract recipe from the provided URL.");

        return ApiResponse<ImportedRecipeData>.Ok(data);
    }
}
