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
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, bool Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            byte ByteValue = 0;

            if (Value)
            {
                ByteValue = 1;
            }

            Write(CodeStream, CodeDataType.Bool, ByteValue);
        }
        /// <summary>
        /// Записать <see cref="Byte"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, byte Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            Write(CodeStream, CodeDataType.Byte, Value);
        }
        /// <summary>
        /// Записать <see cref="Int16"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, short Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            Write(CodeStream, CodeDataType.Short, BitConverter.GetBytes(Value));
        }
        /// <summary>
        /// Записать <see cref="Int32"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, int Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            Write(CodeStream, CodeDataType.Int32, BitConverter.GetBytes(Value));
        }
        /// <summary>
        /// Записать <see cref="Int64"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, long Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            Write(CodeStream, CodeDataType.Int64, BitConverter.GetBytes(Value));
        }
        /// <summary>
        /// Записать <see cref="Char"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, char Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            Write(CodeStream, CodeDataType.Char, BitConverter.GetBytes(Value));
        }
        /// <summary>
        /// Записать <see cref="Double"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, double Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            Write(CodeStream, CodeDataType.Double, BitConverter.GetBytes(Value));
        }
        /// <summary>
        /// Записать <see cref="Single"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, float Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            Write(CodeStream, CodeDataType.Float, BitConverter.GetBytes(Value));
        }
        /// <summary>
        /// Записать <see cref="String"/>
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <param name="Value">Записываемое значение</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(Stream CodeStream, string Value)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }
            if (Value == null)
            {
                throw new ArgumentNullException(nameof(Value));
            }

            byte[] Bytes = new byte[sizeof(int) + sizeof(char) * Value.Length];

            Array.Copy(BitConverter.GetBytes(Value.Length), Bytes, sizeof(int));

            for (int i = 0; i < Bytes.Length; i++)
            {
                int index = sizeof(int) + sizeof(char) * i;
                Array.Copy(BitConverter.GetBytes(Value[i]), 0, Bytes, index, sizeof(int));
            }

            Write(CodeStream, CodeDataType.String, Bytes);
        }

        /// <summary>
        /// Прочитать следующее значение
        /// </summary>
        /// <param name="CodeStream">Код</param>
        /// <returns>Значение одного из типа <see cref="CodeDataType"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static object Read(Stream CodeStream)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            var TypeValue = CodeStream.ReadByte();

            if (TypeValue == -1)
            {
                throw new InvalidDataException("Не удалось получить тип данных");
            }

            return (CodeDataType)TypeValue switch
            {
                CodeDataType.Byte => (byte)CodeStream.ReadByte(),
                CodeDataType.Bool => CodeStream.ReadByte() >= 1,
                CodeDataType.String => ReadString(CodeStream),
                CodeDataType.Short => Read(CodeStream, sizeof(short), BitConverter.ToInt16),
                CodeDataType.Int32 => Read(CodeStream, sizeof(short), BitConverter.ToInt32),
                CodeDataType.Int64 => Read(CodeStream, sizeof(short), BitConverter.ToInt64),
                CodeDataType.Float => Read(CodeStream, sizeof(short), BitConverter.ToSingle),
                CodeDataType.Double => Read(CodeStream, sizeof(short), BitConverter.ToDouble),
                _ => throw new InvalidDataException($"Неизвестный тип: {TypeValue}"),
            };
        }

        private static void Write(Stream CodeStream, CodeDataType Type, params byte[] Bytes)
        {
            CodeStream.WriteByte((byte)Type);

            foreach (var Byte in Bytes)
            {
                CodeStream.WriteByte(Byte);
            }
        }
        private static T Read<T>(Stream CodeStream, int Size, Func<byte[], int, T> Converter)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            byte[] Buffer = new byte[Size];
            var ReadSize = CodeStream.Read(Buffer, 0, Size);

            if (ReadSize != Size)
            {
                throw new InvalidDataException($"Не удалось прочитать данные типа. Прочитано: {ReadSize}, требуется: {Size}");
            }

            return Converter(Buffer, 0);
        }
        private static string ReadString(Stream CodeStream)
        {
            if (CodeStream == null)
            {
                throw new ArgumentNullException(nameof(CodeStream));
            }

            int Length = Read(CodeStream, sizeof(int), BitConverter.ToInt32);

            if (0 >= Length)
            {
                return string.Empty;
            }

            char[] Chars = new char[Length];

            for (int i = 0; i < Length; i++)
            {
                Chars[i] = Read(CodeStream, sizeof(char), BitConverter.ToChar);
            }

            return new(Chars);
        }
    }
}
