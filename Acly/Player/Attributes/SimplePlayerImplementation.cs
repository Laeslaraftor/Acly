using Acly.Platforms;
using System;

namespace Acly.Player
{
    /// <summary>
    /// Пометить класс как реализация <see cref="ISimplePlayer"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SimplePlayerImplementationAttribute : Attribute
    {
        /// <summary>
        /// Пометить класс как реализация <see cref="ISimplePlayer"/>
        /// </summary>
        public SimplePlayerImplementationAttribute()
        {
            Platform = RuntimePlatform.Unknown;
        }
        /// <summary>
        /// Пометить класс как реализация <see cref="ISimplePlayer"/>
        /// </summary>
        /// <param name="platform">Платформа реализации</param>
        public SimplePlayerImplementationAttribute(RuntimePlatform platform)
        {
            Platform = platform;
        }
        /// <summary>
        /// Пометить класс как реализация <see cref="ISimplePlayer"/>
        /// </summary>
        /// <param name="platform">Платформа реализации</param>
        /// <param name="method">Метод для получения экземпляра реализации</param>
        public SimplePlayerImplementationAttribute(RuntimePlatform platform, string method)
        {
            Platform = platform;
            Method = method;
        }

        /// <summary>
        /// Платформа реализации
        /// </summary>
        public RuntimePlatform Platform { get; private set; }
        /// <summary>
        /// Метод для получения экземпляра реализации
        /// </summary>
        public string? Method { get; private set; }
    }
}
