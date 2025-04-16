using DSharpPlus;
using DSharpPlus.Entities;

namespace PlatformBot.Infrastructure.Services.Discord.Components;

/// <summary>
/// Набор компонентов.
/// </summary>
public static class UiComponent
{
    public static readonly DiscordComponent SkipButton =
        new DiscordButtonComponent(ButtonStyle.Secondary, "skip_track", "Пропустить ->");
}