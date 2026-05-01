using System;
using System.Collections.Generic;

namespace Acly
{
    /// <summary>
    /// Класс с методами расширениями для данных
    /// </summary>
    public static class DataExtensions
    {
        extension<T>(ICollection<T> collection)
        {
            /// <summary>
            /// Проверить являются ли коллекции идентичными.
            /// Будет проверяться их длина и содержимые объекты
            /// </summary>
            /// <param name="other">Коллекция для сверки</param>
            /// <returns>Идентичные ли коллекции</returns>
            /// <exception cref="ArgumentNullException"></exception>
            public bool FullyEquals(ICollection<T> other)
            {
                if (other == null)
                {
                    throw new ArgumentNullException(nameof(other));
                }
                if (collection.Count != other.Count)
                {
                    return false;
                }

                foreach (var item in collection)
                {
                    if (!other.Contains(item))
                    {
                        return false;
                    }
                }

                return true;
            }
        }
    }
}
