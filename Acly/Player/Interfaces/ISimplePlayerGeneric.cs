using System.Threading.Tasks;

namespace Acly.Player
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <typeparam name="T">Тип данных для установки как источник аудио</typeparam>
    public interface ISimplePlayer<T> : ISimplePlayer
    {
        /// <summary>
        /// Установить источник из объекта
        /// </summary>
        /// <param name="Data">Объект для установки аудиофайла</param>
        public Task SetSource(T Data);
    }
}
