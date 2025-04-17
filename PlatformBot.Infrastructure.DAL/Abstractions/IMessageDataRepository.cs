namespace PlatformBot.Infrastructure.DAL.Abstractions;

/// <summary>
/// Контракт хранилища данных.
/// </summary>
public interface IMessageDataRepository
{
    /// <summary>
    /// Получение информации по Id.
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="cancellationToken">Токен для отмены операции.</param>
    /// <returns></returns>
    Task<MessageData?> GetByIdAsync(Guid messageId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить изменения.
    /// </summary>
    /// <param name="cancellationToken">Токен для отмены операции.</param>
    /// <returns></returns>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}