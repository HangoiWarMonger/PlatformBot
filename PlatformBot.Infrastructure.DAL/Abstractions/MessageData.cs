using System.Text.Json;

namespace PlatformBot.Infrastructure.DAL.Abstractions;

/// <summary>
/// Данные сообщения.
/// </summary>
public sealed class MessageData : IEquatable<MessageData>
{
    private Dictionary<string, object> _data;

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Данные сообщения.
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public MessageData(Guid id)
    {
        Id = id;
    }

    /// <summary>
    /// Конструктор для EF.
    /// </summary>
    private MessageData()
    {
    }

    /// <summary>
    /// Получение данных.
    /// </summary>
    /// <typeparam name="T">Тип данных.</typeparam>
    /// <returns></returns>
    public T? GetData<T>()
    {
        _data ??= new Dictionary<string, object>();
        var key = typeof(T).AssemblyQualifiedName;
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        if (_data.TryGetValue(key, out var result))
        {
            return result switch
            {
                T typed => typed,
                // Если это JsonElement — пробуем десериализовать
                JsonElement jsonElement => jsonElement.Deserialize<T>(),
                _ => throw new InvalidCastException($"Cannot cast object of type '{result.GetType()}' to '{typeof(T)}'")
            };
        }

        return default;
    }

    /// <summary>
    /// Добавление или обновление данных.
    /// </summary>
    /// <param name="value">Данные.</param>
    /// <typeparam name="T">Тип данных.</typeparam>
    public void AddOrUpdateData<T>(T value) where T : notnull
    {
        _data ??= new Dictionary<string, object>();
        var key = typeof(T).AssemblyQualifiedName;
        ArgumentException.ThrowIfNullOrWhiteSpace(key);

        _data.Add(key, value);
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