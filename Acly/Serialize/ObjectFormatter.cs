using System;
using System.IO;
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
		/// <param name="obj">Объект, который будет сериализован</param>
		/// <param name="stream">Stream в который будет происходить сериализация</param>
		/// <exception cref="ArgumentNullException">Ссылка на <see cref="Stream"/> не указывает на экземпляр</exception>
		public static void Serialize(this object obj, Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "Нельзя сериализовать объект в пустой Stream");
            }

            var data = Formatter.Serialize(obj);
            using MemoryStream memory = new(data);

            memory.CopyTo(stream);
        }
        /// <summary>
        /// Сериализовать объект в файл
        /// </summary>
        /// <param name="obj">Объект, который будет сериализован</param>
        /// <param name="pathToFile">Путь к файлу, в который будет записан результат сериализации. Если файла не существует, то он будет создан, иначе - перезаписан</param>
        public static void Serialize(this object obj, string pathToFile)
        {
            if (pathToFile == null)
            {
                throw new ArgumentNullException(nameof(pathToFile), "Путь к файлу не указан");
            }
            if (File.Exists(pathToFile))
            {
                File.Delete(pathToFile);
            }

            using FileStream stream = File.Open(pathToFile, FileMode.OpenOrCreate);

            obj.Serialize(stream);
        }
        /// <summary>
        /// Сериализовать объект и получить результат
        /// </summary>
        /// <param name="obj">Объект, который будет сериализован</param>
        /// <returns>Результат сериализации</returns>
        public static byte[] Serialize(this object obj)
        {
            using var memory = new MemoryStream();

            obj.Serialize(memory);

            return memory.ToArray();
        }
        /// <summary>
        /// Получить копию объекта
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="obj">Объект, который будет скопирован</param>
        /// <returns>Копия объекта</returns>
        public static T Copy<T>(this T obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj) + " не указан");
            }

            byte[] serializeResult = obj.Serialize();
            return serializeResult.Deserialize<T>();
        }

        #endregion

        #region Десериализация

        /// <summary>
		/// Десериализовать файл
		/// </summary>
		/// <param name="pathToFile">Путь к файлу, который необходимо десериализовать</param>
		/// <returns>Десериализованный объект</returns>
		/// <exception cref="IOException">Файл для десериализации не найден</exception>
		public static T Deserialize<T>(string pathToFile)
        {
            if (!File.Exists(pathToFile))
            {
                throw new IOException("Файл (" + pathToFile + ") не найден");
            }

            using FileStream stream = File.OpenRead(pathToFile);

            return stream.Deserialize<T>();
        }
        /// <summary>
        /// Десериализовать объект из указанного Stream
        /// </summary>
        /// <param name="obj">Stream из которого будет десериализовываться объект</param>
        /// <returns>Десериализованный объект</returns>
        public static T Deserialize<T>(this Stream obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            using MemoryStream memory = new();
            obj.CopyTo(memory);

            return Formatter.Deserialize<T>(memory.ToArray());
        }
        /// <summary>
        /// Десериализовать массив байтов
        /// </summary>
        /// <param name="obj">Массив байтов, который будет десериализован</param>
        /// <returns>Десериализованный объект</returns>
        public static T Deserialize<T>(this byte[] obj)
        {
            using MemoryStream memory = new(obj);
            return memory.Deserialize<T>();
        }

        #endregion
    }
}
