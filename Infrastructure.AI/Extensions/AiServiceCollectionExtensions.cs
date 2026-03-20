using Application.Interfaces;
using Infrastructure.AI.Services;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;

namespace Infrastructure.AI.Extensions;

public static class AiServiceCollectionExtensions
{
    public static IServiceCollection AddAiServices(this IServiceCollection services)
    {
        return services.AddAiServices(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AiOptions>>().Value;
            return new OpenAIClient(options.ApiKey)
                .GetChatClient(options.Model)
                .AsIChatClient();
        });
    }

    public static IServiceCollection AddAiServices(this IServiceCollection services, Func<IServiceProvider, IChatClient> chatClientFactory)
    {
        services.AddSingleton(chatClientFactory);
        services.AddScoped<IAiService, AiService>();

        return services;
    }
}
