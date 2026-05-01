using Acly.Serialize;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acly.Requests
{
    /// <summary>
    /// Информация о сообщении
    /// </summary>
    [Serializable]
    public class MessageData
    {
        /// <summary>
        /// Создать экземпляр информации о сообщении
        /// </summary>
        /// <param name="data">Данные сообщения</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public MessageData(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            int zeroCount = 0;
            List<byte> typeStringBytes = [];
            List<byte> listData = [.. data];

            foreach (var value in data)
            {
                if (value == 0)
                {
                    zeroCount++;
                }

                typeStringBytes.Add(value);

                if (zeroCount == 3)
                {
                    break;
                }
            }

            if (typeStringBytes.Count <= 3)
            {
                throw new ArgumentException("Неверные данные");
            }

            listData.RemoveRange(0, typeStringBytes.Count);
            typeStringBytes.RemoveRange(typeStringBytes.Count - 3, 3);

            Type = DefaultEncoding.GetString([.. typeStringBytes]);
            Data = [.. listData];
        }

        /// <summary>
        /// Тип данных
        /// </summary>
        public string Type { get; private set; }
        /// <summary>
        /// Данные сообщения
        /// </summary>
#pragma warning disable CA1819
        public byte[] Data { get; private set; }
#pragma warning restore CA1819

        #region Статика

        /// <summary>
        /// Кодировка по умолчанию
        /// </summary>       
        public static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// Получить массив байтов объекта
        /// </summary>
        /// <param name="obj">Объект для создания сообщения</param>
        /// <returns>Сообщение в виде массива байтов</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] Create(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            List<byte> result = [.. DefaultEncoding.GetBytes(obj.GetType().FullName)];
            result.AddRange([0, 0, 0]);
            result.AddRange(obj.Serialize());

            return [.. result];
        }

        #endregion
    }
}
