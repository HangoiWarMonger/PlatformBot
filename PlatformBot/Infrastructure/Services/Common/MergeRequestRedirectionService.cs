using System.Text.RegularExpressions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Microsoft.Extensions.Options;
using PlatformBot.Infrastructure.Components;
using PlatformBot.Infrastructure.DAL.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;
using PlatformBot.Infrastructure.Options;

namespace PlatformBot.Infrastructure.Services.Common;

public partial class MergeRequestRedirectionService(
    IOptions<DiscordOptions> options,
    IMessageDataRepository repository
    ) : IMessageHandler
{
    /// <inheritdoc />
    public bool CanHandleMessage(MessageCreateEventArgs args)
    {
        // Проверка, включена ли эта функция.
        if (options.Value.MrRedirection is null)
        {
            return false;
        }

        var content = args.Message.Content;
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }

        return MessageIsMergeRequest(content);
    }

    /// <inheritdoc />
    public async Task HandleMessage(MessageCreateEventArgs args)
    {
        var mergeUrl = MergeRequestRegex().Match(args.Message.Content).Value;

        var id = Guid.NewGuid();
        var messageData = new MessageData
        {
            Data =
            {
                {FieldConstatns.MrLink, mergeUrl}
            }
        };
        await repository.AddAsync(messageData);
        await repository.SaveChangesAsync();

        var messageBuilder = new DiscordMessageBuilder()
            .WithEmbed(Embed.MergeRequestRedirectionAsk(id, mergeUrl))
            .AddComponents(RedirectMrComponent.UiComponent, CancelButton.UiComponent);

        await args.Channel.SendMessageAsync(messageBuilder);
    }

    /// <summary>
    /// Определение, содержит ли в себе сообщение ссылку на MR.
    /// </summary>
    /// <param name="content">Сообщение.</param>
    private static bool MessageIsMergeRequest(string content)
    {
        var match = MergeRequestRegex().Match(content);

        return match.Success;
    }

    [GeneratedRegex(@"https:\/\/git\.dextechnology\.com\/[\w\-\/\.]+\/-\/merge_requests\/\d+")]
    private static partial Regex MergeRequestRegex();
}