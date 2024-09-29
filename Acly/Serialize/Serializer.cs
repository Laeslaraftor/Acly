using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters;

namespace Acly.Serialize
{
	/// <summary>
	/// Класс с методами сериализации объектов
	/// </summary>
	public static class Serializer
	{
		internal readonly static BinaryFormatter Formatter = new()
		{
			AssemblyFormat = FormatterAssemblyStyle.Simple,
			TypeFormat = FormatterTypeStyle.TypesWhenNeeded,
			FilterLevel = TypeFilterLevel.Full,
		};

		/// <summary>
		/// Сериализовать объект в указанный Stream
		/// </summary>
		/// <param name="Obj">Объект, который будет сериализован</param>
		/// <param name="Stream">Stream в который будет происходить сериализация</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <exception cref="ArgumentNullException">Ссылка на <see cref="Stream"/> не указывает на экземпляр</exception>
		public static void Serialize(this object Obj, Stream Stream, SerializationBinder? Binder = null)
		{
			if (Stream == null)
			{
				throw new ArgumentNullException(nameof(Stream), "Нельзя сериализовать объект в пустой Stream");
			}

			if (Binder != null)
			{
				Formatter.Binder = Binder;
			}

			Formatter.Serialize(Stream, Obj);
		}
		/// <summary>
		/// Сериализовать объект в файл
		/// </summary>
		/// <param name="Obj">Объект, который будет сериализован</param>
		/// <param name="PathToFile">Путь к файлу, в который будет записан результат сериализации. Если файла не существует, то он будет создан, иначе - перезаписан</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		public static void Serialize(this object Obj, string PathToFile, SerializationBinder? Binder = null)
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

			Obj.Serialize(Stream, Binder);
		}
		/// <summary>
		/// Сериализовать объект и получить результат
		/// </summary>
		/// <param name="Obj">Объект, который будет сериализован</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Результат сериализации</returns>
		public static byte[] Serialize(this object Obj, SerializationBinder? Binder = null)
		{
			using var Mem = new MemoryStream();

			Obj.Serialize(Mem, Binder);

			return Mem.ToArray();
		}
		/// <summary>
		/// Получить копию объекта
		/// </summary>
		/// <typeparam name="T">Тип объекта</typeparam>
		/// <param name="Obj">Объект, который будет скопирован</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Копия объекта</returns>
		public static T Copy<T>(this T Obj, SerializationBinder? Binder = null)
		{
			if (Obj == null)
			{
				throw new ArgumentNullException(nameof(Obj) + " не указан");
			}

			byte[] SerializeResult = Obj.Serialize(Binder);

			return SerializeResult.Deserialize<T>(Binder);
		}
	}
}
