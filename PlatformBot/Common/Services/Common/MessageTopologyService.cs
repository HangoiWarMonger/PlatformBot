using DSharpPlus.EventArgs;

namespace PlatformBot.Common.Services.Common;

/// <summary>
/// Сервис распределения сообщений.
/// </summary>
/// <param name="messageHandlers">Обработчики.</param>
public class MessageTopologyService(IEnumerable<IMessageHandler> messageHandlers)
{
    /// <summary>
    /// Перенаправление сообщения в нужный обработчик, если такой присутствует.
    /// </summary>
    /// <param name="args">Аргументы.</param>
    public Task MapMessage(MessageCreateEventArgs args)
    {
        var handler = messageHandlers.FirstOrDefault(h => h.CanHandleMessage(args));

        if (handler != null)
        {
            return handler.HandleMessage(args);
        }

        return Task.CompletedTask;
    }
}