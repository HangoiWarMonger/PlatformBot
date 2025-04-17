using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PlatformBot.Infrastructure.DAL.Implementations;

/// <summary>
/// Фабрика создания <see cref="ApplicationDbContext"/> во время работы с миграциями.
/// </summary>
public class ApplicationDbContextDesignTime : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    /// <inheritdoc />
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlite();

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}