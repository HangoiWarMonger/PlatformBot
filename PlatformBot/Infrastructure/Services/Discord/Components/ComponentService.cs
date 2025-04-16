using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Services.Discord.Components;

/// <summary>
/// Сервис управления компонентами.
/// </summary>
public class ComponentService : IComponentConfigurator, IComponentExecutor
{
    private readonly List<InteractiveComponent> _interactions = [];

    /// <inheritdoc />
    public void Register(DiscordComponent discordComponent, Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> action)
    {
        var interactiveComponent = new InteractiveComponent(discordComponent.CustomId, action);
        _interactions.Add(interactiveComponent);
    }

    /// <inheritdoc />
    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args, CancellationToken cancellationToken = default)
    {
        var component = _interactions.FirstOrDefault(x => x.Id == args.Id);

        if (component is null) return;

        await component.Action(client, args);
    }
}