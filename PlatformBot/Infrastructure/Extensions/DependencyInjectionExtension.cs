using Ardalis.GuardClauses;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Options;
using PlatformBot.Infrastructure.Options;
using PlatformBot.Infrastructure.Services.Common;
using PlatformBot.Infrastructure.Services.Discord;
using PlatformBot.Infrastructure.Services.Discord.Components;

namespace PlatformBot.Infrastructure.Extensions;

/// <summary>
/// Методы регистрации сервисов.
/// </summary>
public static class DependencyInjectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MessageTopologyService>();

        services.AddScoped<IMessageHandler, MergeRequestRedirectionService>();

        return services;
    }

    /// <summary>
    /// Добавление дискорд клиента.
    /// </summary>
    /// <param name="services">Набор сервисов.</param>
    public static IServiceCollection AddDiscordClient(this IServiceCollection services)
    {
        services
            .AddOptionsWithValidateOnStart<DiscordOptions>(nameof(DiscordOptions))
            .ValidateOnStart();

        services.AddSingleton<DiscordClient>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<DiscordClient>>();
            var discordOptions = provider.GetRequiredService<IOptions<DiscordOptions>>().Value;
            var componentExecutor = provider.GetRequiredService<IComponentExecutor>();

            Guard.Against.NullOrWhiteSpace(discordOptions.Token);

            var config = new DiscordConfiguration
            {
                Token = discordOptions.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Intents = DiscordIntents.All
            };

            var client = new DiscordClient(config);

            var slashCommands = new SlashCommandsConfiguration
            {
                Services = provider,
            };

            var slashConfig = client.UseSlashCommands(slashCommands);
            slashConfig.RegisterCommands(typeof(Program).Assembly, discordOptions.TrustedGuildId);

            slashConfig.SlashCommandErrored += async (_, args) =>
            {
                logger.LogCritical("Exception: {Name}: {Message}", args.Exception.GetType().Name,
                    args.Exception.Message);
                logger.LogError("{Trace}", args.Exception.StackTrace);

                await args.Context.EditResponseAsync(new DiscordWebhookBuilder()
                    .AddEmbed(Embed.Error(args.Context.Member, args.Exception.Message)));
            };

            client.ComponentInteractionCreated += async (discordClient, args) =>
            {
                if (args.Guild == null || args.Guild.Id != discordOptions.TrustedGuildId)
                {
                    return;
                }

                await componentExecutor.ExecuteAsync(discordClient, args);
            };

            client.MessageCreated += async (_, args) =>
            {
                await using var scope = services.BuildServiceProvider().CreateAsyncScope();
                var endpoint = scope.ServiceProvider.GetRequiredService<MessageTopologyService>();
                await endpoint.MapMessage(args);
            };

            return client;
        });

        return services;
    }
}