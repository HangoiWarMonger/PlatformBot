using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Services.Common;

/// <summary>
/// Контракт обработчика сообщений.
/// </summary>
public interface IMessageHandler
{
    /// <summary>
    /// Может ли быть обработано сообщение.
    /// </summary>
    bool CanHandleMessage(MessageCreateEventArgs args);

    /// <summary>
    /// Обработка сообщения.
    /// </summary>
    Task HandleMessage(MessageCreateEventArgs args);
}