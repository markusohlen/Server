using Application.Models;
using Application.Interfaces;
using Domain.Models;

namespace Application.Services;

public class MealPlannerService
{
    private readonly IMealRepository _mealRepository;

    public MealPlannerService(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
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
}
