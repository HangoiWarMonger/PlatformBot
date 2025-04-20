using DSharpPlus.EventArgs;

namespace PlatformBot.Common.Services.Common;

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