using System.ClientModel;
using Application.Interfaces;
using Infrastructure.AI.Services;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OpenAI;

namespace Infrastructure.AI.Extensions;

public static class AiServiceCollectionExtensions
{
    private static readonly Dictionary<string, Func<AiOptions, IChatClient>> ChatClientFactories = new(StringComparer.OrdinalIgnoreCase)
    {
        ["OpenAI"] = CreateOpenAiClient,
        ["Gemini"] = CreateGeminiClient,
    };

    public static IServiceCollection AddAiServices(this IServiceCollection services)
    {
        return services.AddAiServices(sp =>
        {
            var options = sp.GetRequiredService<IOptions<AiOptions>>().Value;

            if (!ChatClientFactories.TryGetValue(options.Provider, out var factory))
                throw new InvalidOperationException(
                    $"Unsupported AI provider: '{options.Provider}'. Supported providers: {string.Join(", ", ChatClientFactories.Keys)}");

            return factory(options);
        });
    }

    public static IServiceCollection AddAiServices(this IServiceCollection services, Func<IServiceProvider, IChatClient> chatClientFactory)
    {
        services.AddSingleton(chatClientFactory);
        services.AddScoped<IAiService, AiService>();

        return services;
    }

    private static IChatClient CreateOpenAiClient(AiOptions options)
    {
        var client = string.IsNullOrWhiteSpace(options.Endpoint)
            ? new OpenAIClient(options.ApiKey)
            : new OpenAIClient(new ApiKeyCredential(options.ApiKey), new OpenAIClientOptions { Endpoint = new Uri(options.Endpoint) });

        return client.GetChatClient(options.Model).AsIChatClient();
    }

    private static IChatClient CreateGeminiClient(AiOptions options)
    {
        var endpoint = string.IsNullOrWhiteSpace(options.Endpoint)
            ? new Uri("https://generativelanguage.googleapis.com/v1beta/openai/")
            : new Uri(options.Endpoint);

        return new OpenAIClient(new ApiKeyCredential(options.ApiKey), new OpenAIClientOptions { Endpoint = endpoint })
            .GetChatClient(options.Model)
            .AsIChatClient();
    }
}
