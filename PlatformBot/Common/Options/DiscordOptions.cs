using PlatformBot.Features.MergeRequestRedirect.Models;

namespace PlatformBot.Common.Options;

/// <summary>
/// Настройки Discord.
/// </summary>
public class DiscordOptions
{
    /// <summary>
    /// Токен.
    /// </summary>
    public required string Token { get; init; }

    /// <summary>
    /// Id гильдии, на которой будет запущен бот.
    /// (для защитных целей будет работать только на ней).
    /// </summary>
    public required ulong TrustedGuildId { get; init; }

    /// <summary>
    /// Настройки редиректа MR.
    /// </summary>
    public MrRedirectionOptions? MrRedirectionOptions { get; init; }
}