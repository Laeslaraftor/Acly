using System.Collections.Generic;
using System.Collections.ObjectModel;
using System;
using Acly.Serialize;
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
        /// <param name="Data">Данные сообщения</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public MessageData(byte[] Data)
        {
            if (Data == null)
            {
                throw new ArgumentNullException(nameof(Data));
            }

            int ZeroCount = 0;
            List<byte> TypeStringBytes = new();
            List<byte> ListData = new(Data);

            foreach (var Value in Data)
            {
                if (Value == 0)
                {
                    ZeroCount++;
                }

                TypeStringBytes.Add(Value);

                if (ZeroCount == 3)
                {
                    break;
                }
            }

            if (TypeStringBytes.Count <= 3)
            {
                throw new ArgumentException("Неверные данные");
            }

            ListData.RemoveRange(0, TypeStringBytes.Count);
            TypeStringBytes.RemoveRange(TypeStringBytes.Count - 3, 3);

            Type = DefaultEncoding.GetString(TypeStringBytes.ToArray());
            this.Data = ListData.ToArray();
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
        /// <param name="Object">Объект для создания сообщения</param>
        /// <returns>Сообщение в виде массива байтов</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static byte[] Create(object Object)
        {
            if (Object == null)
            {
                throw new ArgumentNullException(nameof(Object));
            }

            List<byte> Result = new(DefaultEncoding.GetBytes(Object.GetType().FullName));
            Result.AddRange(new byte[] { 0, 0, 0 });
            Result.AddRange(Object.Serialize());

            return Result.ToArray();
        }

        #endregion
    }
}
