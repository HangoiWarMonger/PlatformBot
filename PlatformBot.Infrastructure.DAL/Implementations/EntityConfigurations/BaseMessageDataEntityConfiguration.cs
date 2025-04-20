using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformBot.Infrastructure.DAL.Abstractions;

namespace PlatformBot.Infrastructure.DAL.Implementations.EntityConfigurations;

/// <summary>
/// базовый класс конфигурации сообщений.
/// </summary>
public abstract class BaseMessageDataEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : MessageData
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired()
            .HasColumnName(nameof(MessageData.Id))
            .ValueGeneratedNever();

        ConfigureEntity(builder);
    }

    /// <summary>
    /// Конфигурация сущности.
    /// </summary>
    protected abstract void ConfigureEntity(EntityTypeBuilder<T> builder);
}