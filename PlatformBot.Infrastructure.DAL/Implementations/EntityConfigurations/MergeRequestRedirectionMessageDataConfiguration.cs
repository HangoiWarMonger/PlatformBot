using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatformBot.Infrastructure.DAL.Abstractions;

namespace PlatformBot.Infrastructure.DAL.Implementations.EntityConfigurations;

public class MergeRequestRedirectionMessageDataConfiguration : BaseMessageDataEntityConfiguration<MergeRequestRedirectionMessageData>
{
    /// <inheritdoc />
    protected override void ConfigureEntity(EntityTypeBuilder<MergeRequestRedirectionMessageData> builder)
    {
        builder.ToTable(nameof(MergeRequestRedirectionMessageData));

        builder.Property(x => x.MergeRequestId)
            .HasColumnName(nameof(MergeRequestRedirectionMessageData.MergeRequestId))
            .IsRequired();

        builder.Property(x => x.MergeRequestUrl)
            .HasColumnName(nameof(MergeRequestRedirectionMessageData.MergeRequestUrl))
            .IsRequired();

        builder.OwnsOne(x => x.RequestMessageLocation, locationBuilder =>
        {
            locationBuilder.Property(x => x.ChannelId)
                .IsRequired();

            locationBuilder.Property(x => x.MessageId)
                .IsRequired();
        });

        builder.OwnsOne(x => x.RedirectMessageLocation, redirectLocationBuilder =>
        {
            redirectLocationBuilder.Property(x => x.ChannelId)
                .IsRequired(false);

            redirectLocationBuilder.Property(x => x.MessageId)
                .IsRequired(false);
        });
    }
}