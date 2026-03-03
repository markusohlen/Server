using Application.Interfaces;
using Application.Models;
using Database.EfCore.Context;
using Domain.Models;
using Domain.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public class MealRepository : IMealRepository
{
    private readonly MealPlannerDbContext _context;

    public MealRepository(MealPlannerDbContext context)
    {
        _context = context;
    }

    public async Task<List<Guid>> GetAllMealIdsAsync()
    {
        return await _context.Meals
            .Select(m => m.Id)
            .ToListAsync();
    }
    // Fetch all meals with their ingredients and instructions
    public async Task<List<Meal>> GetAllMealsAsync()
    {
        return await _context.Meals
            .Include(m => m.Ingredients)
            .Include(m => m.Instructions)
                .ThenInclude(s => s.InstructionIngredients)
                    .ThenInclude(ii => ii.Ingredient)
            .ToListAsync();
    }

    // Fetch a meal by its ID
    public async Task<MealView?> GetMealByIdAsync(Guid id)
    {
        var meal = await _context.Meals
            .Include(m => m.Ingredients)
            .Include(m => m.Instructions)
                .ThenInclude(s => s.InstructionIngredients)
                    .ThenInclude(ii => ii.Ingredient)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (meal == null) return null;

        return ToMealView(meal);
    }

    // Fetch all meals as MealView
    public async Task<List<MealView>> GetAllMealViewsAsync()
    {
        var meals = await _context.Meals
            .Include(m => m.Ingredients)
            .Include(m => m.Instructions)
                .ThenInclude(s => s.InstructionIngredients)
                    .ThenInclude(ii => ii.Ingredient)
            .AsNoTracking()
            .ToListAsync();

        return meals.Select(ToMealView).ToList();
    }

    // Add a new meal to the database
    public async Task AddMealAsync(Meal meal)
    {
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();
    }

    // Update an existing meal
    public async Task UpdateMealAsync(Meal meal)
    {
        _context.Meals.Update(meal);
        await _context.SaveChangesAsync();
    }

    // Delete a meal
    public async Task DeleteMealAsync(Guid id)
    {
        var meal = await _context.Meals.FirstOrDefaultAsync(m => m.Id == id);
        if (meal != null)
        {
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
        }
    }

    private static MealView ToMealView(Meal m) =>
        new MealView
        {
            Id = m.Id,
            Name = m.Name,
            Ingredients = m.Ingredients.Select(ToIngredientsView).ToList(),
            Instructions = m.Instructions
                .OrderBy(s => s.StepNumber)
                .Select(ToInstructionStepView)
                .ToList(),
            Portions = m.Portions,
            TotalCalories = m.Ingredients.Sum(i => i.Calories),
            TotalProtein = m.Ingredients.Sum(i => i.Protein)
        };

    private static IngredientsView ToIngredientsView(Ingredient i) =>
        new IngredientsView
        {
            Id = i.Id,
            Name = i.Name,
            Calories = i.Calories,
            Protein = i.Protein,
            Price = i.Price,
            Volume = i.Volume,
            Measure = i.Measure.ToSwedishTranslation()
        };

    private static InstructionStepView ToInstructionStepView(InstructionStep s) =>
        new InstructionStepView
        {
            Id = s.Id,
            StepNumber = s.StepNumber,
            Text = s.Text,
            Ingredients = s.InstructionIngredients
                .OrderBy(ii => ii.SortOrder)
                .Select(ii => new InstructionIngredientView
                {
                    IngredientId = ii.IngredientId,
                    Name = ii.Ingredient.Name,
                    Amount = ii.Amount,
                    Measure = ii.Measure.ToSwedishTranslation()
                })
                .ToList()
        };
}
