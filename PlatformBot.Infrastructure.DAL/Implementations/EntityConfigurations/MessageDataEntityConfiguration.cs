using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformBot.Infrastructure.DAL.Abstractions;

namespace PlatformBot.Infrastructure.DAL.Implementations.EntityConfigurations;

/// <summary>
/// Конфигурация <see cref="MessageData"/> в базе данных.
/// </summary>
public class MessageDataEntityConfiguration : IEntityTypeConfiguration<MessageData>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<MessageData> builder)
    {
        builder.ToTable(nameof(MessageData));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName(nameof(MessageData.Id))
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property<Dictionary<string, object>>("_data")
            .HasColumnName("Data")
            .IsRequired()
            .HasColumnType("jsonb")
            .HasConversion(
                v => JsonSerializer.Serialize(v, new JsonSerializerOptions {WriteIndented = false}),
                v => JsonSerializer.Deserialize<Dictionary<string, object>>(v, new JsonSerializerOptions())
                     ?? new Dictionary<string, object>());
    }
}