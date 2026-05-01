using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Acly
{
    public static partial class Helper
    {
        /// <summary>
        /// Конвертировать <see cref="Stream"/> в массив байтов
        /// </summary>
        /// <param name="stream"><see cref="Stream"/>, конвертируемый в массив байтов</param>
        /// <returns>Массив байтов</returns>
        public static async Task<byte[]> ToBytesAsync(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "Трансляция не указана");
            }

            using MemoryStream memory = new();
            await stream.CopyToAsync(memory, 0);

            return memory.ToArray();
        }
        /// <summary>
        /// Конвертировать <see cref="Stream"/> в массив байтов
        /// </summary>
        /// <param name="stream"><see cref="Stream"/>, конвертируемый в массив байтов</param>
        /// <returns>Массив байтов</returns>
        public static byte[] ToBytes(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "Трансляция не указана");
            }

            using MemoryStream memory = new();
            stream.CopyTo(memory, 0);

            return memory.ToArray();
        }
        /// <summary>
        /// Преобразовать массив байтов в <see cref="Stream"/>
        /// </summary>
        /// <param name="bytes">Массив байтов для преобразования</param>
        /// <returns>Трансляция массива байтов</returns>
        /// <exception cref="ArgumentNullException">Массив байтов не указан</exception>
        public static Stream ToStream(this byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes), "Массив байтов не указан");
            }

            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Конвертировать массив символов в строку
        /// </summary>
        /// <param name="charArray">Массив символов</param>
        /// <param name="trim">Обрезать ли лишние пробелы вначале и в конце строки</param>
        /// <returns>Массив символов как строка</returns>
        /// <exception cref="ArgumentNullException">Массив символов не задан</exception>
        public static string ToString(this char[] charArray, bool trim)
        {
            if (charArray == null)
            {
                throw new ArgumentNullException(nameof(charArray), "Массив символов не задан");
            }

            string Result = new(charArray);

            if (trim)
            {
                Result = Result.Trim();
            }

            return Result;
        }
        /// <summary>
        /// Получить MD5 хэш-код строки
        /// </summary>
        /// <param name="str">Хэшируемая строка</param>
        /// <returns>MD5 хэш-код строки</returns>
        public static string MD5(this string str)
        {
#pragma warning disable CA5351
#pragma warning disable CA1308
#pragma warning disable CA1307
            using MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = Encoding.Unicode.GetBytes(str);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLowerInvariant();
#pragma warning restore CA1307
#pragma warning restore CA1308
#pragma warning restore CA5351
        }
    }
}
