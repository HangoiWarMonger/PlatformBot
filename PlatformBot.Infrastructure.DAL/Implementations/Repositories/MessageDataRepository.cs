using Microsoft.EntityFrameworkCore;
using PlatformBot.Infrastructure.DAL.Abstractions;

namespace PlatformBot.Infrastructure.DAL.Implementations.Repositories;

/// <summary>
/// Репозиторий для <see cref="MessageData"/>.
/// </summary>
/// <param name="dbContext">Контекст базы данных.</param>
public class MessageDataRepository(ApplicationDbContext dbContext) : IMessageDataRepository
{
    /// <inheritdoc />
    public Task<MessageData?> GetByIdAsync(Guid messageId, bool asNoTracking = false, CancellationToken cancellationToken = default)
    {
        var messageData = dbContext.Messages.Where(m => m.Id == messageId);

        if (asNoTracking)
        {
            messageData = messageData.AsNoTracking();
        }

        return messageData.SingleOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc />
    public async Task AddAsync(MessageData message, CancellationToken cancellationToken = default)
    {
        await dbContext.Messages.AddAsync(message, cancellationToken);
    }

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}