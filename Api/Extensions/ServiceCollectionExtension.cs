using Application.Interfaces;
using Application.Services;
using Domain.Repositories;

namespace Api.Extensions;

public static class ServiceCollectionExtension
{
    public static void RegisterServices(this IServiceCollection service)
    {
        service.AddScoped<IMealRepository, MealRepository>();
    }

    public static void RegisterRepositories(this IServiceCollection service)
    {
        service.AddScoped<MealPlannerService>();
    }
}
