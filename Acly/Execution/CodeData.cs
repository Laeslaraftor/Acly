using System;
using System.IO;

namespace Acly.Execution
{
    /// <summary>
    /// Статический класс с методами чтения/записи данных в байт-код
    /// </summary>
    public static class CodeData
    {
        /// <summary>
        /// Записать <see cref="Boolean"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, bool value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            byte byteValue = 0;

            if (value)
            {
                byteValue = 1;
            }

            Write(codeStream, CodeDataType.Bool, byteValue);
        }
        /// <summary>
        /// Записать <see cref="Byte"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, byte value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            Write(codeStream, CodeDataType.Byte, value);
        }
        /// <summary>
        /// Записать <see cref="Int16"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, short value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            Write(codeStream, CodeDataType.Short, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Записать <see cref="Int32"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, int value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            Write(codeStream, CodeDataType.Int32, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Записать <see cref="Int64"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, long value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            Write(codeStream, CodeDataType.Int64, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Записать <see cref="Char"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, char value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            Write(codeStream, CodeDataType.Char, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Записать <see cref="Double"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, double value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            Write(codeStream, CodeDataType.Double, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Записать <see cref="Single"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, float value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            Write(codeStream, CodeDataType.Float, BitConverter.GetBytes(value));
        }
        /// <summary>
        /// Записать <see cref="String"/>
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <param name="value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream codeStream, string value)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            byte[] bytes = new byte[sizeof(int) + sizeof(char) * value.Length];

            Array.Copy(BitConverter.GetBytes(value.Length), bytes, sizeof(int));

            for (int i = 0; i < bytes.Length; i++)
            {
                int index = sizeof(int) + sizeof(char) * i;
                Array.Copy(BitConverter.GetBytes(value[i]), 0, bytes, index, sizeof(int));
            }

            Write(codeStream, CodeDataType.String, bytes);
        }

        /// <summary>
        /// Прочитать следующее значение
        /// </summary>
        /// <param name="codeStream">Код</param>
        /// <returns>Значение одного из типа <see cref="CodeDataType"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object Read(Stream codeStream)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            var typeValue = codeStream.ReadByte();

            if (typeValue == -1)
            {
                throw new InvalidDataException("Не удалось получить тип данных");
            }

            return (CodeDataType)typeValue switch
            {
                CodeDataType.Byte => (byte)codeStream.ReadByte(),
                CodeDataType.Bool => codeStream.ReadByte() >= 1,
                CodeDataType.String => ReadString(codeStream),
                CodeDataType.Short => Read(codeStream, sizeof(short), BitConverter.ToInt16),
                CodeDataType.Int32 => Read(codeStream, sizeof(short), BitConverter.ToInt32),
                CodeDataType.Int64 => Read(codeStream, sizeof(short), BitConverter.ToInt64),
                CodeDataType.Float => Read(codeStream, sizeof(short), BitConverter.ToSingle),
                CodeDataType.Double => Read(codeStream, sizeof(short), BitConverter.ToDouble),
                _ => throw new InvalidDataException($"Неизвестный тип: {typeValue}"),
            };
        }

        private static void Write(Stream codeStream, CodeDataType type, params byte[] bytes)
        {
            codeStream.WriteByte((byte)type);

            foreach (var Byte in bytes)
            {
                codeStream.WriteByte(Byte);
            }
        }
        private static T Read<T>(Stream codeStream, int size, Func<byte[], int, T> converter)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            byte[] buffer = new byte[size];
            var readSize = codeStream.Read(buffer, 0, size);

            if (readSize != size)
            {
                throw new InvalidDataException($"Не удалось прочитать данные типа. Прочитано: {readSize}, требуется: {size}");
            }

            return converter(buffer, 0);
        }
        private static string ReadString(Stream codeStream)
        {
            if (codeStream == null)
            {
                throw new ArgumentNullException(nameof(codeStream));
            }

            int length = Read(codeStream, sizeof(int), BitConverter.ToInt32);

            if (0 >= length)
            {
                return string.Empty;
            }

            Span<char> chars = stackalloc char[Math.Min(length, 1024)];

            if (length > 1024)
            {
                chars = new char[length];
            }

            for (int i = 0; i < length; i++)
            {
                chars[i] = Read(codeStream, sizeof(char), BitConverter.ToChar);
            }

            return new(chars);
        }
    }
}
