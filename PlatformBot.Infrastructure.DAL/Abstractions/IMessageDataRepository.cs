namespace PlatformBot.Infrastructure.DAL.Abstractions;

/// <summary>
/// Контракт хранилища данных.
/// </summary>
public interface IMessageDataRepository
{
    /// <summary>
    /// Получение информации по Id.
    /// </summary>
    /// <param name="messageId">Id сообщения.</param>
    /// <param name="asNoTracking">Использовать ли трекер.</param>
    /// <param name="cancellationToken">Токен для отмены операции.</param>
    /// <returns></returns>
    Task<MessageData?> GetByIdAsync(Guid messageId, bool asNoTracking = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить изменения.
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены операции.</param>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}