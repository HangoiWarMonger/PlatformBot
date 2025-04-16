using DSharpPlus;
using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Services.Discord.Components;

/// <summary>
/// Контракт сервиса вызова действий компонентов.
/// </summary>
public interface IComponentExecutor
{
    /// <summary>
    /// Вызов связанного с компонентом действия.
    /// </summary>
    /// <param name="client">Дискорд клиент.</param>
    /// <param name="args">Аргументы вызова.</param>
    /// <param name="cancellationToken">Токен для отмены операции.</param>
    Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args, CancellationToken cancellationToken = default);
}