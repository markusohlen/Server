using Application.Models;
using Domain.Models;

namespace Application.Interfaces;

public interface IMealRepository
{
    Task<List<Guid>> GetAllMealIdsAsync();
    Task<List<Meal>> GetAllMealsAsync();
    Task<List<MealView>> GetAllMealViewsAsync();
    Task<MealView?> GetMealByIdAsync(Guid id);
    Task AddMealAsync(Meal meal);
    Task UpdateMealAsync(Meal meal);
    Task DeleteMealAsync(Guid id);
}
