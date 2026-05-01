using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Acly
{
    public static partial class Helper
    {
        /// <summary>
        /// Конвертировать записную книжку только для чтения в обычную
        /// </summary>
        /// <typeparam name="TKey">Тип данных ключа</typeparam>
        /// <typeparam name="TValue">Тип данных значения</typeparam>
        /// <param name="original">Записная книжка только для чтения</param>
        /// <returns>Записная книжка</returns>
        /// <exception cref="ArgumentNullException">Записная книжка не указана</exception>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> original)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original), "Словарь не указан");
            }

            return new(original);
        }
        /// <summary>
        /// Сохранить состояние объекта
        /// </summary>
        /// <param name="obj">Объект для сохранения состояния</param>
        /// <returns>Сохранённое состояние</returns>
        public static ObjectState SaveState(this object obj) => new(obj);
        /// <summary>
        /// Сохранить состояние объекта асинхронно
        /// </summary>
        /// <param name="obj">Объект для сохранения состояния</param>
        /// <returns>Сохранённое состояние</returns>
        public static async Task<ObjectState> SaveStateAsync(this object obj) => await Task.Run(() => obj.SaveState());

        /// <summary>
        /// Скопировать один поток в другой с отслеживанием прогресса
        /// </summary>
        /// <param name="source">Поток из которого будут копироваться данные</param>
        /// <param name="destination">Поток в который будут записываться скопированные данные</param>
        /// <param name="bufferSize">Размер временного буфера (шаг копирования). Чем больше значение, тем больше итераций чтения и записи</param>
        /// <param name="progress">Объект, который будет получать количество скопированных байтов</param>
        /// <param name="cancellationToken">Токен отмены</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static async Task CopyToAsync(this Stream source, Stream destination, int bufferSize = 1024, IProgress<long>? progress = null, CancellationToken cancellationToken = default)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (!source.CanRead)
            {
                throw new ArgumentException("Источник должен быть читаемым", nameof(source));
            }
            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }
            if (!destination.CanWrite)
            {
                throw new ArgumentException("Поток назначения должна быть доступна для записи", nameof(destination));
            }
            if (bufferSize < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(bufferSize));
            }

            byte[] buffer = new byte[bufferSize];
            long totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer, cancellationToken).ConfigureAwait(false)) != 0)
            {
                await destination.WriteAsync(buffer, cancellationToken).ConfigureAwait(false);

                totalBytesRead += bytesRead;
                progress?.Report(totalBytesRead);
            }
        }
    }
}
