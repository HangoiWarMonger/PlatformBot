namespace PlatformBot.Features.MergeRequestRedirect.Models;

/// <summary>
/// Настройки перенаправления все MR в один канал на ревью.
/// </summary>
public class MrRedirectionOptions
{
    /// <summary>
    /// Канал для перенаправления сообщений.
    /// </summary>
    public required ulong RedirectionChannelId { get; init; }
}