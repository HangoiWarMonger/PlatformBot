using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Options;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;
using PlatformBot.Infrastructure.Options;

namespace PlatformBot.Infrastructure.Components;

public class ChooseMergeRequestReviewerComponent(
    IOptions<DiscordOptions> options
) : IComponent
{
    /// <inheritdoc />
    public static DiscordComponent UiComponent { get; } =
        new DiscordUserSelectComponent(
            customId: "select_user",
            placeholder: "Выбери участника",
            minOptions: 1,
            maxOptions: 10
        );

    /// <inheritdoc />
    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        var id = args.Message.GetInteractionId();
        await UiComponentHelper.DefferAsync(id, args.Interaction);

        var channelId = options.Value.MrRedirection.RedirectionChannelId;
        var channel = await client.GetChannelAsync(channelId);

        var chooses = args.Values.Select(x => $"<@{x}>").ToList();
        chooses.Add($"<@{args.User.Id}>");

        var author = args.User.Id;
        await channel.SendMessageAsync(new DiscordMessageBuilder()
            .AddEmbed(Embed.ReviewerSend(id, author))
            .WithAllowedMentions(Mentions.All)
            .AddComponents(MrReviewedButton.UiComponent)
            .WithContent(string.Join(" ", chooses)));

        await args.Interaction.DeleteOriginalResponseAsync();
    }
}