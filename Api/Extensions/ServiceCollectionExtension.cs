using Application.Interfaces;
using Application.Services;
using Domain.Repositories;
using Infrastructure.AI;
using Infrastructure.AI.Extensions;
using Infrastructure.Services;

namespace Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterServices(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddScoped<IMealRepository, MealRepository>();
        service.AddHttpClient<IIngredientLookupService, IngredientLookupService>();
        service.AddHttpClient<IRecipeImportService, RecipeImportService>();

        service.Configure<AiOptions>(configuration.GetSection(AiOptions.SectionName));
        service.AddAiServices();
    }

    public static void RegisterRepositories(this IServiceCollection service)
    {
        service.AddScoped<MealPlannerService>();
    }
}
