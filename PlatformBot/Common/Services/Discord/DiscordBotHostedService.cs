using Ardalis.GuardClauses;
using DSharpPlus;

namespace PlatformBot.Common.Services.Discord;

/// <summary>
/// Сервис запуска бота.
/// </summary>
/// <param name="logger">Логгер.</param>
/// <param name="client">Дискорд клиент.</param>
public class DiscordBotHostedService(ILogger<DiscordBotHostedService> logger, DiscordClient client) : IHostedService
{
    private readonly ILogger<DiscordBotHostedService> _logger = Guard.Against.Null(logger);
    private readonly DiscordClient _client = Guard.Against.Null(client);

    /// <inheritdoc />
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Запускаем бота.");
        await _client.ConnectAsync();
        _logger.LogInformation("Бот успешно запущен.");
    }

    /// <inheritdoc />
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Выключаем бота.");
        await _client.DisconnectAsync();
        _logger.LogInformation("Бот успешно выключен.");
    }
}