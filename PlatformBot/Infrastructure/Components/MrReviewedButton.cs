using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Infrastructure.Components;

public class MrReviewedButton : IComponent
{
    /// <inheritdoc />
    public static DiscordComponent UiComponent { get; } =
        new DiscordButtonComponent(ButtonStyle.Success, "merge_reviewed", "Закрыть проверку.");

    /// <inheritdoc />
    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        // TODO: сделать обратное сообщение в ветку.
        await UiComponentHelper.DefferAsync(args.Interaction);
        await args.Interaction.DeleteOriginalResponseAsync();
    }
}