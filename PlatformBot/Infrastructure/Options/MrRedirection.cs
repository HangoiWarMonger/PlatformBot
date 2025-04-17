namespace PlatformBot.Infrastructure.Options;

/// <summary>
/// Настройки перенаправления все MR в один канал на ревью.
/// </summary>
public class MrRedirection
{
    /// <summary>
    /// Канал для перенаправления сообщений.
    /// </summary>
    public required ulong RedirectionChannelId { get; init; }
}