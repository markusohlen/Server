using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public record ImportRecipeRequest(string Url);

[Route("mealplanner")]
[ApiController]
public class MealPlannerController : ControllerBase
{
    private readonly MealPlannerService _mealService;

    public MealPlannerController(MealPlannerService mealPlannerService)
    {
        _mealService = mealPlannerService;
    }

    [HttpGet("getallmealids")]
    public async Task<ActionResult<Guid>> GetMealById()
    {
        var mealIds = await _mealService.GetAllMealIdsAsync();
        if (mealIds == null)
        {
            return NotFound(new { Message = $"No meal was found." });
        }
        return Ok(mealIds);
    }

    [HttpGet("getallmeals")]
    public async Task<ActionResult<List<MealView>>> GetAllMeals()
    {
        var response = await _mealService.GetAllMealViewsAsync();
        if (response == null || response.Data == null)
        {
            return NotFound(new { Message = $"No meals were found." });
        }
        return Ok(response.Data);
    }

    [HttpGet("getmealbyid/{id}")]
    public async Task<ActionResult<MealView>> GetMealById(Guid id)
    {
        var response = await _mealService.GetMealByIdResponseAsync(id);
        if (response == null || response.Data == null)
        {
            return NotFound(new { Message = $"Meal with id {id} not found." });
        }
        return Ok(response.Data);
    }

    [HttpGet("getallingredientnames")]
    public async Task<ActionResult<List<string>>> GetAllIngredientNames()
    {
        var response = await _mealService.GetAllIngredientNamesAsync();
        return Ok(response.Data);
    }

    [HttpGet("lookupingredient/{name}")]
    public async Task<ActionResult<IngredientNutritionData>> LookupIngredient(string name)
    {
        var response = await _mealService.LookupIngredientNutritionAsync(name);
        if (!response.Success || response.Data == null)
        {
            return NotFound(new { Message = $"Nutrition data not found for '{name}'." });
        }
        return Ok(response.Data);
    }

    [HttpPost("importrecipe")]
    public async Task<ActionResult<ImportedRecipeData>> ImportRecipe([FromBody] ImportRecipeRequest request)
    {
        var response = await _mealService.ImportRecipeAsync(request.Url);
        if (!response.Success || response.Data == null)
        {
            return BadRequest(new { Message = response.Message });
        }
        return Ok(response.Data);
    }

    [HttpPost("addmeal")]
    public async Task<ActionResult<MealView>> AddMeal([FromBody] Meal meal)
    {
        //var meal = await _mealService.GetMealByIdAsync(id);
        //if (meal == null)
        //{
        //    return NotFound(new { Message = $"Meal with id {id} not found." });
        //}

        await _mealService.AddMealAsync(meal);
        return Ok();
    }
}

