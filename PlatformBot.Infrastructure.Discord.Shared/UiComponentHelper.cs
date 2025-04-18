using DSharpPlus;
using DSharpPlus.Entities;

namespace PlatformBot.Infrastructure.Discord.Shared;

public static class UiComponentHelper
{
    /// <summary>
    /// Получение идентификатора взаимодействия. Нужен для получения данных из хранилища.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    public static Guid GetInteractionId(this DiscordMessage message)
    {
        var embed = message.Embeds[0];
        var footer = embed.Footer.Text;
        return Guid.Parse(footer);
    }

    public static async Task<DiscordMessage> DefferAsync(Guid id, DiscordInteraction interaction)
    {
        await interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

        return await interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(Embed.Info(id, "Обработка команды...", "Сейчас закончим!")));
    }
}