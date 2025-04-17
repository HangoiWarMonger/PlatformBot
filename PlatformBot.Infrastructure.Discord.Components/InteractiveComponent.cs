using DSharpPlus;
using DSharpPlus.EventArgs;

namespace PlatformBot.Infrastructure.Discord.Components;

/// <summary>
/// Хранение данных компонента.
/// </summary>
/// <param name="Id">Идентификатор.</param>
/// <param name="Action">Действие компонента.</param>
internal record InteractiveComponent(string Id, Func<DiscordClient, ComponentInteractionCreateEventArgs, Task> Action);