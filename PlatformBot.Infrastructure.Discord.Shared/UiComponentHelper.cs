using System.Text.Json;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Discord.Shared;

public static class UiComponentHelper
{
    public static T? TryGetFieldFromMessageAsync<T>(ComponentInteractionCreateEventArgs args, string fieldName, Func<string, T?>? parser = null)
    {
        var embed = args.Message.Embeds.FirstOrDefault();

        var field = embed?.Fields.FirstOrDefault(field => field.Name == fieldName);
        if (field == null)
        {
            return default;
        }

        var raw = field.Value.Replace("```", "").Trim();

        try
        {
            var result = parser != null ? parser(raw) : JsonSerializer.Deserialize<T>(raw);

            return result;
        }
        catch
        {
            return default;
        }
    }

    public static async Task<DiscordMessage> DefferAsync(DiscordInteraction interaction)
    {
        await interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);

        return await interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(Embed.Info("Обработка команды...", "Сейчас закончим!")));
    }
}