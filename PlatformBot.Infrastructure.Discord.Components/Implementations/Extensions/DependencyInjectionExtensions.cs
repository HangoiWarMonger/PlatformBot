using System.Reflection;
using DSharpPlus.Entities;
using Microsoft.Extensions.DependencyInjection;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;

namespace PlatformBot.Infrastructure.Discord.Components.Implementations.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Регистрация компонентов.
    /// </summary>
    /// <param name="services">Сервисы.</param>
    /// <param name="configure">Конфигуратор.</param>
    public static IServiceCollection AddUiComponents(this IServiceCollection services, Action<ComponentConfig> configure)
    {
        var config = new ComponentConfig();
        configure(config);
        foreach (var type in config.Types)
        {
            services.AddScoped(type);
        }

        services.AddSingleton<ComponentService>(provider =>
        {
            var componentService = new ComponentService();
            foreach (var componentType in config.Types)
            {
                var uiComponentProp = componentType.GetProperty(nameof(IComponent.UiComponent),
                    BindingFlags.Static | BindingFlags.Public);

                if (uiComponentProp == null)
                    throw new InvalidOperationException(
                        $"{componentType.Name} должен иметь публичное статическое свойство UiComponent.");

                var uiComponent = (DiscordComponent)uiComponentProp.GetValue(null)!;

                componentService.Register(uiComponent, async (client, args) =>
                {
                    await using var scope = provider.CreateAsyncScope();
                    var requiredService = (IComponent) scope.ServiceProvider.GetRequiredService(componentType);
                    await requiredService.ExecuteAsync(client, args);
                });
            }

            return componentService;
        });

        services.AddSingleton<IComponentExecutor>(provider => provider.GetRequiredService<ComponentService>());

        return services;
    }
}