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
		/// <param name="Stream"><see cref="Stream"/>, конвертируемый в массив байтов</param>
		/// <returns>Массив байтов</returns>
		public static async Task<byte[]> ToBytesAsync(this Stream Stream)
		{
			if (Stream == null)
			{
				throw new ArgumentNullException(nameof(Stream), "Трансляция не указана");
			}

			using MemoryStream Mem = new();
			await Stream.CopyToAsync(Mem, 0);

			return Mem.ToArray();
		}
        /// <summary>
        /// Конвертировать <see cref="Stream"/> в массив байтов
        /// </summary>
        /// <param name="Stream"><see cref="Stream"/>, конвертируемый в массив байтов</param>
        /// <returns>Массив байтов</returns>
        public static byte[] ToBytes(this Stream Stream)
        {
            if (Stream == null)
            {
                throw new ArgumentNullException(nameof(Stream), "Трансляция не указана");
            }

            using MemoryStream Mem = new();
            Stream.CopyTo(Mem, 0);

            return Mem.ToArray();
        }
        /// <summary>
        /// Преобразовать массив байтов в <see cref="Stream"/>
        /// </summary>
        /// <param name="Bytes">Массив байтов для преобразования</param>
        /// <returns>Трансляция массива байтов</returns>
        /// <exception cref="ArgumentNullException">Массив байтов не указан</exception>
        public static Stream ToStream(this byte[] Bytes)
		{
			if (Bytes == null)
			{
				throw new ArgumentNullException(nameof(Bytes), "Массив байтов не указан");
			}

			return new MemoryStream(Bytes);
		}

		/// <summary>
		/// Конвертировать массив символов в строку
		/// </summary>
		/// <param name="CharArray">Массив символов</param>
		/// <param name="Trim">Обрезать ли лишние пробелы вначале и в конце строки</param>
		/// <returns>Массив символов как строка</returns>
		/// <exception cref="ArgumentNullException">Массив символов не задан</exception>
		public static string ToString(this char[] CharArray, bool Trim)
		{
			if (CharArray == null)
			{
				throw new ArgumentNullException(nameof(CharArray), "Массив символов не задан");
			}

			string Result = "";

			foreach (var Char in CharArray)
			{
				Result += Char;
			}

			if (Trim)
			{
				Result = Result.Trim();
			}

			return Result;
		}
		/// <summary>
		/// Получить MD5 хэш-код строки
		/// </summary>
		/// <param name="Str">Хэшируемая строка</param>
		/// <returns>MD5 хэш-код строки</returns>
		public static string MD5(this string Str)
		{
#pragma warning disable CA5351
#pragma warning disable CA1308
#pragma warning disable CA1307
			using MD5 Md5 = System.Security.Cryptography.MD5.Create();
			byte[] InputBytes = Encoding.Unicode.GetBytes(Str);
			byte[] HashBytes = Md5.ComputeHash(InputBytes);

			return BitConverter.ToString(HashBytes).Replace("-", "").ToLowerInvariant();
#pragma warning restore CA1307
#pragma warning restore CA1308
#pragma warning restore CA5351
		}
	}
}
