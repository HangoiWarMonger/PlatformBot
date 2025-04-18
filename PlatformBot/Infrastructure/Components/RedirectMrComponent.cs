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
        var id = args.Message.GetInteractionId();
        await UiComponentHelper.DefferAsync(id, args.Interaction);

        await args.Interaction.DeleteOriginalResponseAsync();

        var response = new DiscordMessageBuilder()
            .WithAllowedMentions(Mentions.None)
            .AddEmbed(Embed.ReviewerAsk(id))
            .AddComponents(ChooseMergeRequestReviewerComponent.UiComponent);

        await args.Channel.SendMessageAsync(response);
    }
}