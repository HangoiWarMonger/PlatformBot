using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PlatformBot.Features.MergeRequestRedirect.Services;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Features.MergeRequestRedirect.Components;

public class ChooseMergeRequestReviewerComponent(
    MrRedirectionService service
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

        await service.ChooseMrReviewersAndSendAsync(id, args.Values, args.User.Id);

        await args.Interaction.EditOriginalResponseAsync(new DiscordWebhookBuilder()
            .AddEmbed(Embed.Info(id, "Ваш MR был отправлен на проверку.")));
    }
}