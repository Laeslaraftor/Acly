using System.IO;
using System.Runtime.Serialization;

namespace Acly.Serialize
{
	/// <summary>
	/// Класс для десериализации файлов и данных
	/// </summary>
	public static class Deserializer
	{
		/// <summary>
		/// Десериализовать файл
		/// </summary>
		/// <param name="PathToFile">Путь к файлу, который необходимо десериализовать</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Десериализованный объект</returns>
		/// <exception cref="IOException">Файл для десериализации не найден</exception>
		public static object Deserialize(string PathToFile, SerializationBinder? Binder = null)
		{
			if (!File.Exists(PathToFile))
			{
				throw new IOException("Файл (" + PathToFile + ") не найден");
			}

			using FileStream Stream = File.OpenRead(PathToFile);

			return Stream.Deserialize(Binder);
		}
		/// <summary>
		/// Десериализовать файл и привести результат к указанному типу
		/// </summary>
		/// <typeparam name="T">Тип, к которому будет приведён результат десериализации</typeparam>
		/// <param name="PathToFile">Путь к файлу, который необходимо десериализовать</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Десериализованный объект, приведённый к указанному типу</returns>
		public static T Deserialize<T>(string PathToFile, SerializationBinder? Binder = null)
		{
			return (T)Deserialize(PathToFile, Binder);
		}

		/// <summary>
		/// Десериализовать объект из указанного Stream
		/// </summary>
		/// <param name="Obj">Stream из которого будет десериализовываться объект</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Десериализованный объект</returns>
		public static object Deserialize(this Stream Obj, SerializationBinder? Binder = null)
		{
			if (Binder != null)
			{
				Serializer.Formatter.Binder = Binder;
			}

#pragma warning disable CA2302
			return Serializer.Formatter.Deserialize(Obj);
#pragma warning restore CA2302
		}
		/// <summary>
		/// Десериализовать объект и привести его в указанный тип из Stream
		/// </summary>
		/// <typeparam name="T">Тип, в которой будет приведён десериализованный объект</typeparam>
		/// <param name="Obj">Stream из которого будет десериализовываться объект</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Десериализованный и приведённый объект</returns>
		public static T Deserialize<T>(this Stream Obj, SerializationBinder? Binder = null)
		{
			return (T)Obj.Deserialize(Binder);
		}
		/// <summary>
		/// Десериализовать массив байтов
		/// </summary>
		/// <param name="Obj">Массив байтов, который будет десериализован</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Десериализованный объект</returns>
		public static object Deserialize(this byte[] Obj, SerializationBinder? Binder = null)
		{
			using MemoryStream Mem = new MemoryStream(Obj);
			return Mem.Deserialize(Binder);
		}
		/// <summary>
		/// Десериализовать массив байтов и привести результат к указанному типу
		/// </summary>
		/// <typeparam name="T">Тип, к которому будет приведён результат</typeparam>
		/// <param name="Obj">Массив байтов, который будет десериализован</param>
		/// <param name="Binder">Штука для настройки сериализации</param>
		/// <returns>Десериализованный объект приведённый к указанному типу</returns>
		public static T Deserialize<T>(this byte[] Obj, SerializationBinder? Binder = null)
		{
			return (T)Obj.Deserialize(Binder);
		}
	}
}
