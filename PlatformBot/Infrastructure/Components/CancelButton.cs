using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;
using PlatformBot.Infrastructure.Discord.Shared;

namespace PlatformBot.Infrastructure.Components;

/// <summary>
/// Стандартная кнопка отмены. Убирает сообщение.
/// </summary>
public class CancelButton : IComponent
{
    /// <inheritdoc />
    public static DiscordComponent UiComponent { get; } =
        new DiscordButtonComponent(ButtonStyle.Secondary, "cancel_button", "Отмена");

    /// <inheritdoc />
    public async Task ExecuteAsync(DiscordClient client, ComponentInteractionCreateEventArgs args)
    {
        await UiComponentHelper.DefferAsync(args.Interaction);
        await args.Interaction.DeleteOriginalResponseAsync();
    }
}