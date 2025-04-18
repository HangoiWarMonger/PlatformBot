namespace PlatformBot.Infrastructure.Dto;

public class MergeRedirectDto
{
    public string MergeRequestUrl { get; init; } = string.Empty;
    public string OriginalMessageUrl { get; init; } = string.Empty;
}