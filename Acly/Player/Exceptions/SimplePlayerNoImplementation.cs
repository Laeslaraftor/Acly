using Acly.Platforms;
using System;

namespace Acly.Player
{
    /// <summary>
    /// Исключение, вызывающееся при отсутствии реализаций <see cref="ISimplePlayer"/>
    /// </summary>
    /// <remarks>
    /// Вызвать исключение отсутствия реализаций
    /// </remarks>
    /// <param name="platform">Требуемая платформа</param>
    public sealed class SimplePlayerNoImplementation(RuntimePlatform platform)
        : Exception(string.Format(_message, platform))
    {
        private const string _message = "Для платформы {0} отсутствует реализация";
    }
}
