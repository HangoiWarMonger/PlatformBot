namespace PlatformBot.Infrastructure.DAL.Abstractions;

/// <summary>
/// Данные сообщения.
/// </summary>
public abstract class MessageData : IEquatable<MessageData>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Данные сообщения.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    protected MessageData(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Конструктор для EF.
    /// </summary>
    protected MessageData()
    {
    }

    /// <inheritdoc />
    public bool Equals(MessageData? other)
    {
        return Id == other?.Id;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is MessageData data && Equals(data);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}