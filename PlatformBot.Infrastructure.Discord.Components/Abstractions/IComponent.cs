using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Discord.Components.Abstractions;

/// <summary>
/// Контракт компонента.
/// </summary>
public interface IComponent
{
    /// <summary>
    /// Визуальный стиль элемента.
    /// </summary>
    static abstract DiscordComponent UiComponent { get; }

    /// <summary>
    /// Обработка действий.
    /// </summary>
    /// <param name="client">Клиент.</param>
    /// <param name="args">Аргументы.</param>
    Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args);
}