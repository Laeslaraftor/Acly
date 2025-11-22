using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace Acly.Serialize
{
    /// <summary>
    /// Класс с методами расширения для сериализации/десериализации
    /// </summary>
    public static class ObjectFormatter
    {
        /// <summary>
        /// Сериализатор/десериализатор
        /// </summary>
#pragma warning disable CS8601
        public static IFormatter Formatter { get; set; } = StandardFormatter;
#pragma warning restore CS8601
        /// <summary>
        /// Стандартный сериализатор/десериализатор на основе <see cref="BinaryFormatter"/>
        /// </summary>
        public static readonly IFormatter StandardFormatter = new StandardFormatter();

        #region Сериализация

        /// <summary>
		/// Сериализовать объект в указанный Stream
		/// </summary>
		/// <param name="Obj">Объект, который будет сериализован</param>
		/// <param name="Stream">Stream в который будет происходить сериализация</param>
		/// <exception cref="ArgumentNullException">Ссылка на <see cref="Stream"/> не указывает на экземпляр</exception>
		public static void Serialize(this object Obj, Stream Stream)
        {
            if (Stream == null)
            {
                throw new ArgumentNullException(nameof(Stream), "Нельзя сериализовать объект в пустой Stream");
            }

            var Data = Formatter.Serialize(Obj);
            using MemoryStream Memory = new(Data);

            Memory.CopyTo(Stream);
        }
        /// <summary>
        /// Сериализовать объект в файл
        /// </summary>
        /// <param name="Obj">Объект, который будет сериализован</param>
        /// <param name="PathToFile">Путь к файлу, в который будет записан результат сериализации. Если файла не существует, то он будет создан, иначе - перезаписан</param>
        public static void Serialize(this object Obj, string PathToFile)
        {
            if (PathToFile == null)
            {
                throw new ArgumentNullException(nameof(PathToFile), "Путь к файлу не указан");
            }

            if (File.Exists(PathToFile))
            {
                File.Delete(PathToFile);
            }

            using FileStream Stream = File.Open(PathToFile, FileMode.OpenOrCreate);

            Obj.Serialize(Stream);
        }
        /// <summary>
        /// Сериализовать объект и получить результат
        /// </summary>
        /// <param name="Obj">Объект, который будет сериализован</param>
        /// <returns>Результат сериализации</returns>
        public static byte[] Serialize(this object Obj)
        {
            using var Mem = new MemoryStream();

            Obj.Serialize(Mem);

            return Mem.ToArray();
        }
        /// <summary>
        /// Получить копию объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="Obj">Объект, который будет скопирован</param>
        /// <returns>Копия объекта</returns>
        public static T Copy<T>(this T Obj)
        {
            if (Obj == null)
            {
                throw new ArgumentNullException(nameof(Obj) + " не указан");
            }

            byte[] SerializeResult = Obj.Serialize();

            return SerializeResult.Deserialize<T>();
        }

        #endregion

        #region Десериализация

        /// <summary>
		/// Десериализовать файл
		/// </summary>
		/// <param name="PathToFile">Путь к файлу, который необходимо десериализовать</param>
		/// <returns>Десериализованный объект</returns>
		/// <exception cref="IOException">Файл для десериализации не найден</exception>
		public static T Deserialize<T>(string PathToFile)
        {
            if (!File.Exists(PathToFile))
            {
                throw new IOException("Файл (" + PathToFile + ") не найден");
            }

            using FileStream Stream = File.OpenRead(PathToFile);

            return Stream.Deserialize<T>();
        }
        /// <summary>
        /// Десериализовать объект из указанного Stream
        /// </summary>
        /// <param name="Obj">Stream из которого будет десериализовываться объект</param>
        /// <returns>Десериализованный объект</returns>
        public static T Deserialize<T>(this Stream Obj)
        {
            if (Obj == null)
            {
                throw new ArgumentNullException(nameof(Obj));
            }

            using MemoryStream Memory = new();
            Obj.CopyTo(Memory);

            return Formatter.Deserialize<T>(Memory.ToArray());
        }
        /// <summary>
        /// Десериализовать массив байтов
        /// </summary>
        /// <param name="Obj">Массив байтов, который будет десериализован</param>
        /// <returns>Десериализованный объект</returns>
        public static T Deserialize<T>(this byte[] Obj)
        {
            using MemoryStream Mem = new(Obj);
            return Mem.Deserialize<T>();
        }

        #endregion
    }
}
