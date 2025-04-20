using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PlatformBot.Features.MergeRequestRedirect.Services;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Features.MergeRequestRedirect.Components;

public class RedirectMrComponent(MrRedirectionService service) : IComponent
{
    /// <inheritdoc />
    public static DiscordComponent UiComponent { get; } =
        new DiscordButtonComponent(ButtonStyle.Success, "redirect_merge_button", "Да");

    /// <inheritdoc />
    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        var id = args.Message.GetInteractionId();
        await UiComponentHelper.DefferAsync(id, args.Interaction);

        await args.Interaction.DeleteOriginalResponseAsync();
        await service.AskIfUserWantsReviewAsync(id, args.Channel);
    }
}