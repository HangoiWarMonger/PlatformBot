using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Infrastructure.Components;

public class RedirectMrComponent : IComponent
{
    /// <inheritdoc />
    public static DiscordComponent UiComponent { get; } =
        new DiscordButtonComponent(ButtonStyle.Success, "redirect_merge_button", "Да");

    /// <inheritdoc />
    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        await UiComponentHelper.DefferAsync(args.Interaction);
        await args.Interaction.DeleteOriginalResponseAsync();
        Console.WriteLine("Привет");
    }
}