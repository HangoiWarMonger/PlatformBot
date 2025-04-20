using DSharpPlus.EventArgs;
using PlatformBot.Common.Services.Common;
using PlatformBot.Features.MergeRequestRedirect.Services;

namespace PlatformBot.Features.MergeRequestRedirect.Messaging;

public class MergeRequestRedirectionService(MrRedirectionService service) : IMessageHandler
{
    /// <inheritdoc />
    public bool CanHandleMessage(MessageCreateEventArgs args)
    {
        if (service.MrRedirectionIsEnabled())
        {
            return false;
        }

        var content = args.Message.Content;

        return service.MessageIsMergeRequest(content);
    }

    /// <inheritdoc />
    public async Task HandleMessage(MessageCreateEventArgs args)
    {
        await service.ProcessMessageWithMrUrl(args.Channel, args.Message);
    }
}