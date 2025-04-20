using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PlatformBot.Features.MergeRequestRedirect.Services;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Features.MergeRequestRedirect.Components;

public class MrReviewedButton(MrRedirectionService service) : IComponent
{
    /// <inheritdoc />
    public static DiscordComponent UiComponent { get; } =
        new DiscordButtonComponent(ButtonStyle.Success, "merge_reviewed", "Закрыть проверку.");

    /// <inheritdoc />
    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        if (!UserWasMentionedInMessage(args))
        {
            return;
        }

        var id = args.Message.GetInteractionId();
        await UiComponentHelper.DefferAsync(id, args.Interaction);
        await args.Interaction.DeleteOriginalResponseAsync();

        await service.MrReviewedAsync(id);
    }

    /// <summary>
    /// Проверка на то, что пользователь был упомянут в сообщении.
    /// </summary>
    private static bool UserWasMentionedInMessage(ComponentInteractionCreateEventArgs args)
    {
        var members = args.Message.MentionedUsers.Select(u => u.Id);

        var actorId = args.User.Id;

        if (members.Contains(actorId))
        {
            return true;
        }

        return false;
    }
}