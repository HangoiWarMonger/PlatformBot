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

        var mergeUrl = UiComponentHelper.TryGetFieldFromMessageAsync(args, FieldConstatns.MrLink, x => x);

        await args.Interaction.DeleteOriginalResponseAsync();

        var a = new DiscordMessageBuilder()
            .WithAllowedMentions(Mentions.None)
            .AddEmbed(Embed.ReviewerAsk(mergeUrl!))
            .AddComponents(ChooseMergeRequestReviewerComponent.UiComponent);

        await args.Channel.SendMessageAsync(a);
    }
}