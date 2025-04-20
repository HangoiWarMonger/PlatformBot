using System.Text.RegularExpressions;
using Ardalis.GuardClauses;
using DSharpPlus;
using DSharpPlus.Entities;
using GitLab.Client.Refit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PlatformBot.Common.Components;
using PlatformBot.Common.Options;
using PlatformBot.Features.MergeRequestRedirect.Components;
using PlatformBot.Infrastructure.DAL.Abstractions;
using PlatformBot.Infrastructure.DAL.Implementations;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Features.MergeRequestRedirect.Services;

/// <summary>
/// Главный сервис по выполнению действий.
/// </summary>
/// <param name="client"></param>
/// <param name="dbContext"></param>
/// <param name="options"></param>
public partial class MrRedirectionService(
    DiscordClient client,
    ApplicationDbContext dbContext,
    IOptions<DiscordOptions> options,
    IGitLabApi gitLabApi)
{
    /// <summary>
    /// Проверка на то, включена ли эта опция.
    /// </summary>
    /// <returns></returns>
    public bool MrRedirectionIsEnabled()
    {
        return options.Value.MrRedirectionOptions is null;
    }

    /// <summary>
    /// Определение, содержит ли в себе сообщение ссылку на MR.
    /// </summary>
    /// <param name="content">Сообщение.</param>
    public bool MessageIsMergeRequest(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }

        var match = MergeRequestRegex().Match(content);

        return match.Success;
    }

    /// <summary>
    /// Обработка сообщения со ссылкой на MR.
    /// </summary>
    /// <param name="channel">Канал.</param>
    /// <param name="message">Сообщение.</param>
    public async Task ProcessMessageWithMrUrl(DiscordChannel channel, DiscordMessage message)
    {
        Guard.Against.Null(message);
        Guard.Against.Null(channel);

        var match = MergeRequestRegex().Match(message.Content);
        var mergeUrl = match.Value;

        var projectPath = match.Groups["projectPath"].Value;
        var mrIid = match.Groups["iid"].Value;

        var mergeRequest = await gitLabApi.GetMergeRequestByProjectAsync(projectPath, mrIid);
        ArgumentNullException.ThrowIfNull(mergeRequest);

        var id = Guid.NewGuid();
        var messageData = new MergeRequestRedirectionMessageData(id, new MessageLocation(message.Id, channel.Id), mergeUrl, mergeRequest.Id);
        await dbContext.MrRedirectionMessages.AddAsync(messageData);
        await dbContext.SaveChangesAsync();

        var messageBuilder = new DiscordMessageBuilder()
            .WithEmbed(Embed.MergeRequestRedirectionAsk(id, mergeUrl))
            .AddComponents(RedirectMrComponent.UiComponent, CancelButton.UiComponent);

        await channel.SendMessageAsync(messageBuilder);
    }

    /// <summary>
    /// Запрос у пользователя, хочет ли он отправить MR на ревью.
    /// </summary>
    /// <param name="interactionId"></param>
    /// <param name="channel"></param>
    public async Task AskIfUserWantsReviewAsync(Guid interactionId, DiscordChannel channel)
    {
        var response = new DiscordMessageBuilder()
            .WithAllowedMentions(Mentions.None)
            .AddEmbed(Embed.ReviewerAsk(interactionId))
            .AddComponents(ChooseMergeRequestReviewerComponent.UiComponent);

        await channel.SendMessageAsync(response);
    }

    /// <summary>
    /// Выбор ревьюеров.
    /// </summary>
    /// <param name="interactionId"></param>
    /// <param name="mentionsIds"></param>
    /// <param name="userId"></param>
    public async Task ChooseMrReviewersAndSendAsync(Guid interactionId, IEnumerable<string> mentionsIds, ulong userId)
    {
        Guard.Against.Default(interactionId);
        Guard.Against.Default(userId);

        if (!MrRedirectionIsEnabled())
        {
            return;
        }

        var channelId = options.Value.MrRedirectionOptions!.RedirectionChannelId;
        var channel = await client.GetChannelAsync(channelId);

        var message = await dbContext.MrRedirectionMessages
            .Include(x => x.RequestMessageLocation)
            .FirstAsync(x => x.Id == interactionId);

        ArgumentNullException.ThrowIfNull(message.RequestMessageLocation.ChannelId);
        ArgumentNullException.ThrowIfNull(message.RequestMessageLocation.MessageId);

        var chooses = mentionsIds.Select(x => $"<@{x}>").ToList();
        chooses.Add($"<@{userId}>");

        var discordChannel = await client.GetChannelAsync(message.RequestMessageLocation.ChannelId.Value);
        var discordMessage = await discordChannel.GetMessageAsync(message.RequestMessageLocation.MessageId.Value);

        var redirectMessage = await channel.SendMessageAsync(new DiscordMessageBuilder()
            .AddEmbed(Embed.ReviewerSend(interactionId, message.MergeRequestUrl, userId, discordMessage.JumpLink.ToString()))
            .WithAllowedMentions(Mentions.All)
            .AddComponents(MrReviewedButton.UiComponent)
            .WithContent(string.Join(" ", chooses)));

        message.RedirectMessageLocation = new MessageLocation(redirectMessage.Id, redirectMessage.ChannelId);
        await dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// MR был проверен.
    /// </summary>
    /// <param name="interactionId"></param>
    public async Task MrReviewedAsync(Guid interactionId)
    {
        var message = await dbContext.MrRedirectionMessages.FirstOrDefaultAsync(x => x.Id == interactionId);
        ArgumentNullException.ThrowIfNull(message);

        await MrReviewedAsync(message);
    }

    /// <summary>
    /// MR был проверен.
    /// </summary>
    /// <param name="mergeRequestId"></param>
    public async Task MrReviewedAsync(int mergeRequestId)
    {
        var message = await dbContext.MrRedirectionMessages.FirstOrDefaultAsync(x => x.MergeRequestId == mergeRequestId);
        ArgumentNullException.ThrowIfNull(message);

        await MrReviewedAsync(message);
    }

    private async Task MrReviewedAsync(MergeRequestRedirectionMessageData mrMessage)
    {
        Guard.Against.Null(mrMessage);
        Guard.Against.Null(mrMessage.RequestMessageLocation);
        Guard.Against.Null(mrMessage.RequestMessageLocation.ChannelId);
        Guard.Against.Null(mrMessage.RequestMessageLocation.MessageId);
        Guard.Against.Null(mrMessage.RedirectMessageLocation);
        Guard.Against.Null(mrMessage.RedirectMessageLocation.MessageId);
        Guard.Against.Null(mrMessage.RedirectMessageLocation.ChannelId);

        var channel = await client.GetChannelAsync(mrMessage.RequestMessageLocation.ChannelId.Value);
        await channel.SendMessageAsync(new DiscordMessageBuilder()
            .WithEmbed(Embed.Info(mrMessage.Id,
                "MR проверен.",
                $"[Merge Request]({mrMessage.MergeRequestUrl}) проверен и слит.")));

        var redirectChannel = await client.GetChannelAsync(mrMessage.RedirectMessageLocation.ChannelId.Value);
        var message = await redirectChannel.GetMessageAsync(mrMessage.RedirectMessageLocation.MessageId.Value);
        await message.DeleteAsync();
    }

    [GeneratedRegex(@"https:\/\/git\.dextechnology\.com\/(?<projectPath>[\w\-\/\.]+)\/-\/merge_requests\/(?<iid>\d+)")]
    private static partial Regex MergeRequestRegex();
}