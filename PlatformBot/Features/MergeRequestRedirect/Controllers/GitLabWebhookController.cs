using Ardalis.GuardClauses;
using GitLab.Contracts.Webhook;
using Microsoft.AspNetCore.Mvc;
using PlatformBot.Features.MergeRequestRedirect.Services;

namespace PlatformBot.Features.MergeRequestRedirect.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class GitLabWebhookController(
    MrRedirectionService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> GetMergeRequestsAsync([FromBody] GitLabMergeRequestWebhookPayload payload)
    {
        Guard.Against.Null(payload);
        Guard.Against.Null(payload.ObjectAttributes);

        var merged = payload.ObjectAttributes.Action is "merge" or "close";
        if (!merged)
        {
            return Ok();
        }

        await service.MrReviewedAsync(payload.ObjectAttributes.Id);

        return Ok();
    }
}