namespace PlatformBot.Infrastructure.DAL.Abstractions;

public class MergeRequestRedirectionMessageData : MessageData
{
    /// <summary>
    /// 
    /// </summary>
    public string MergeRequestUrl { get; set; }

    public MessageLocation RequestMessageLocation { get; set; }

    public MessageLocation? RedirectMessageLocation { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int MergeRequestId { get; set; }

    public MergeRequestRedirectionMessageData(Guid id, MessageLocation requestMessageLocation, string mergeRequestUrl, int mergeRequestId) : base(id)
    {
        MergeRequestUrl = mergeRequestUrl;
        RequestMessageLocation = requestMessageLocation;
        MergeRequestId = mergeRequestId;
    }

    /// <summary>
    /// Конструктор для EF.
    /// </summary>
    private MergeRequestRedirectionMessageData()
    {
    }
}