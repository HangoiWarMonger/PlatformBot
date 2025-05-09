using Ardalis.GuardClauses;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using GitLab.Client.Extensions;
using GitLab.Client.Options;
using Microsoft.Extensions.Options;
using PlatformBot.Common.Options;
using PlatformBot.Common.Services.Common;
using PlatformBot.Features.MergeRequestRedirect.Messaging;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Components.Implementations.Extensions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Common.Extensions;

/// <summary>
/// Методы регистрации сервисов.
/// </summary>
public static class DependencyInjectionExtension
{
    /// <summary>
    /// Добавление GitLab Api.
    /// </summary>
    /// <param name="services">Сервисы.</param>
    /// <param name="configuration">Конфигурация.</param>
    public static IServiceCollection AddGitLabApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GitLabOptions>(configuration.GetSection(nameof(GitLabOptions)));
        var gitLabOptions = configuration.GetSection(nameof(GitLabOptions)).Get<GitLabOptions>();
        ArgumentNullException.ThrowIfNull(gitLabOptions);

        services.AddGitLab(gitLabOptions);

        return services;
    }

    /// <summary>
    /// Добавление сервисов Discord.
    /// </summary>
    /// <param name="services">Сервисы.</param>
    /// <param name="configuration">Конфигурация.</param>
    public static IServiceCollection AddDiscordServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<MessageTopologyService>();

        services.AddScoped<IMessageHandler, MergeRequestRedirectionService>();

        services.AddUiComponents(cfg =>
        {
            cfg.LoadFromAssembly(typeof(Program).Assembly);
        });

        services.AddDiscordClient(configuration);

        return services;
    }

    /// <summary>
    /// Добавление дискорд клиента.
    /// </summary>
    /// <param name="services">Набор сервисов.</param>
    public static IServiceCollection AddDiscordClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DiscordOptions>(configuration.GetSection(nameof(DiscordOptions)));

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
                logger.LogError(args.Exception,"Ошибка во время выполнения: {Message}", args.Exception.Message);

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
                if (args.Author.IsBot)
                {
                    return;
                }

                await using var scope = provider.CreateAsyncScope();
                var endpoint = scope.ServiceProvider.GetRequiredService<MessageTopologyService>();
                await endpoint.MapMessage(args);
            };

            return client;
        });

        return services;
    }
}