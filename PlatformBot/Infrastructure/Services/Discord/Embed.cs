using DSharpPlus.Entities;

namespace PlatformBot.Infrastructure.Services.Discord;

/// <summary>
/// Embed элементы в ответах Discord.
/// </summary>
public static class Embed
{
    /// <summary>
    /// Информационный embed.
    /// </summary>
    /// <param name="info">Информация.</param>
    /// <returns>Embed.</returns>
    public static DiscordEmbed Info(string info)
    {
        return new DiscordEmbedBuilder()
            .WithDescription(info)
            .WithColor(DiscordColor.DarkGray)
            .Build();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="member"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    public static DiscordEmbed Error(DiscordMember member, string error)
    {
        return new DiscordEmbedBuilder
            {
                Title = "Ошибка!",
                Color = DiscordColor.IndianRed
            }
            .AddField("Сообщение", error, inline: true)
            .WithFooter($"Для {member.DisplayName}", member.AvatarUrl)
            .WithTimestamp(DateTime.UtcNow);
    }
}