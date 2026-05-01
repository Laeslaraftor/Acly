using System;

namespace Acly.Platforms
{
    /// <summary>
    /// Платформа, на которой выполняется приложение
    /// </summary>
    [Flags]
    public enum RuntimePlatform
    {
        /// <summary>
        /// Windows
        /// </summary>
        Windows = 1,
        /// <summary>
        /// Android
        /// </summary>
        Android = 2,
        /// <summary>
        /// IOS
        /// </summary>
        IOS = 4,
        /// <summary>
        /// MacOS
        /// </summary>
        MacOS = 8,
        /// <summary>
        /// Linux
        /// </summary>
        Linux = 16,
        /// <summary>
        /// Неизвестная платформа
        /// </summary>
        Unknown = 32
    }
}
