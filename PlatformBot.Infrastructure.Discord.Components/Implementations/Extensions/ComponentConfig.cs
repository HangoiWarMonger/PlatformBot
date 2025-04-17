using System.Reflection;
using PlatformBot.Infrastructure.Discord.Components.Abstractions;

namespace PlatformBot.Infrastructure.Discord.Components.Implementations.Extensions;

public class ComponentConfig
{
    private readonly List<Type> _types = [];

    public IEnumerable<Type> Types => _types.AsReadOnly();

    internal ComponentConfig()
    {
    }

    /// <summary>
    /// Добавить обработчики из Assembly.
    /// </summary>
    /// <param name="assembly">Сборка.</param>
    public ComponentConfig LoadFromAssembly(Assembly assembly)
    {
        var components = assembly.GetTypes().Where(x => x.GetInterfaces().Contains(typeof(IComponent)));
        _types.AddRange(components);

        return this;
    }

    /// <summary>
    /// Добавить обработчик.
    /// </summary>
    /// <typeparam name="T">Обработчик.</typeparam>
    /// <returns></returns>
    public ComponentConfig Add<T>() where T : IComponent
    {
        _types.Add(typeof(T));
        return this;
    }
}