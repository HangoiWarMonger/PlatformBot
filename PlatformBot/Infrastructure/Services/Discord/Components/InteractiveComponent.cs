using DSharpPlus;
using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Services.Discord.Components;

public class InteractiveComponent
{
    public string Id { get; init; }
    public Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> Action { get; init; }

    public InteractiveComponent(string id, Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> action)
    {
        Id = id;
        Action = action;
    }
}