using Microsoft.EntityFrameworkCore;
using PlatformBot.Infrastructure.DAL.Abstractions;

namespace PlatformBot.Infrastructure.DAL.Implementations;

/// <summary>
/// Контекст для работы с базой данных.
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Сообщения.
    /// </summary>
    public DbSet<MergeRequestRedirectionMessageData> MrRedirectionMessages => Set<MergeRequestRedirectionMessageData>();

    /// <summary>
    /// Контекст для работы с базой данных.
    /// </summary>
    /// <param name="options">Настройки.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}