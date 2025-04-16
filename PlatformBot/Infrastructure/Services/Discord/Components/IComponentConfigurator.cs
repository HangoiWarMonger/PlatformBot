using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Services.Discord.Components;

/// <summary>
/// Контракт сервиса для регистрации интерактивных компонентов.
/// </summary>
public interface IComponentConfigurator
{
    /// <summary>
    /// Привязка действия для компонента.
    /// </summary>
    /// <param name="discordComponent">Компонент.</param>
    /// <param name="action">Действие.</param>
    void Register(DiscordComponent discordComponent, Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> action);
}