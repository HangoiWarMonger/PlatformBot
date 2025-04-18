using System.Text;
using DSharpPlus.Entities;

namespace PlatformBot.Infrastructure.Discord.Shared;

/// <summary>
/// Embed элементы в ответах Discord.
/// </summary>
public static class Embed
{
    /// <summary>
    /// Необходим для отправки пустой разделительной строки в описании, так как
    /// Дискорд делает .Trim() для строки перед отправкой.
    /// </summary>
    private const string EmptySymbol = "ㅤ";

    private const string ErrorLiteral = "Ошибка.";

    public static DiscordEmbed MergeRequestRedirectionAsk(Guid id, string mergeRequestUrl)
    {
        return new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Green)
            .WithDescription($"""
                               ## В вашем сообщении обнаружена ссылка на MR.
                               Хотите отправить его на ревью?
                               {mergeRequestUrl}
                               """)
            .WithFooter(id.ToString())
            .Build();
    }

    public static DiscordEmbed ReviewerAsk(Guid id)
    {
        return new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Green)
            .WithDescription("""
                              ## Выберите проверяющих для вашего MR.
                              """)
            .WithFooter(id.ToString())
            .Build();
    }

    public static DiscordEmbed ReviewerSend(Guid id, string mergeRequestUrl, ulong author, string originalMessageUri)
    {
        var response = new StringBuilder();

        response.AppendLine($"## Запрошено ревью MR от <@{author}>")
            .AppendLine()
            .AppendLine(mergeRequestUrl)
            .AppendLine($"Ссылка на сообщение: {originalMessageUri}");

        response.AppendLine(EmptySymbol);

        response.AppendLine();
        return new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Yellow)
            .WithDescription(response.ToString())
            .WithFooter(id.ToString())
            .Build();
    }

    /// <summary>
    /// Создание ответа, содержащего информацию.
    /// </summary>
    /// <param name="title">Заголовок.</param>
    /// <param name="description">Информация.</param>
    /// <returns>Embed для отправки его в ответ.</returns>
    public static DiscordEmbed Info(Guid id, string title, string description)
    {
        return new DiscordEmbedBuilder()
            .WithTitle(title)
            .WithDescription(description)
            .WithColor(DiscordColor.Cyan)
            .WithFooter(id.ToString())
            .Build();
    }

    /// <summary>
    /// Информационный embed.
    /// </summary>
    /// <param name="info">Информация.</param>
    /// <returns>Embed.</returns>
    public static DiscordEmbed Info(Guid id, string info)
    {
        return new DiscordEmbedBuilder()
            .WithDescription(info)
            .WithColor(DiscordColor.DarkGray)
            .WithFooter(id.ToString())
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

    /// <summary>
    /// Создание ответа - ошибки.
    /// </summary>
    /// <param name="description">Информация.</param>
    /// <returns>Embed для отправки его в ответ.</returns>
    public static DiscordEmbed Error(string description)
    {
        return new DiscordEmbedBuilder()
            .WithDescription($"""
                              ### {ErrorLiteral}
                              **{description}**
                              """)
            .WithColor(DiscordColor.IndianRed)
            .Build();
    }
}