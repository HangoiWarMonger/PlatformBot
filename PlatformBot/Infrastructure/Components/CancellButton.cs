using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Infrastructure.Components;

public class CancellButton : IComponent
{
    public static DiscordComponent UiComponent { get; } =
        new DiscordButtonComponent(ButtonStyle.Secondary, "cancel_button", "Отмена");

    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        await UiComponentHelper.DefferAsync(args.Interaction);
        await args.Interaction.DeleteOriginalResponseAsync();
    }
}