using Application.Models;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

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

