using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlatformBot.Infrastructure.DAL.Abstractions;
using PlatformBot.Infrastructure.DAL.Implementations.Repositories;

namespace PlatformBot.Infrastructure.DAL.Implementations.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Добавление хранилища данных в DI.
    /// </summary>
    /// <param name="services">Сервисы.</param>
    /// <param name="configuration">Конфигурация.</param>
    public static IServiceCollection AddDal(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddSqlite<ApplicationDbContext>($"Data Source={connectionString}");

        services.AddScoped<IMessageDataRepository, MessageDataRepository>();

        return services;
    }
}