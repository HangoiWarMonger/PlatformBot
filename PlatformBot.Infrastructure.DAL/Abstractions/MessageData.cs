namespace PlatformBot.Infrastructure.DAL.Abstractions;

/// <summary>
/// Данные сообщения.
/// </summary>
public class MessageData : IEquatable<MessageData>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Данные.
    /// </summary>
    public Dictionary<string, object> Data { get; private set; }

    /// <summary>
    /// Конструктор для EF core.
    /// </summary>
    private MessageData()
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
        if (obj is null) return false;

        return obj.GetType() == GetType() && Equals((MessageData) obj);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}