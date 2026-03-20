using Application.Models;

namespace Application.Interfaces;

public interface IIngredientLookupService
{
    Task<IngredientNutritionData?> LookupNutritionAsync(string ingredientName);
}
