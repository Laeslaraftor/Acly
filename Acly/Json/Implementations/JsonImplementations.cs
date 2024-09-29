using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.JsonData.Implementations
{
	/// <summary>
	/// Класс для поиска реализации <see cref="IJsonConverter"/>
	/// </summary>
	public static class JsonImplementations
	{
		private static IEnumerable<Type>? _Implementations;

		/// <summary>
		/// Получить тип объекта, помеченного как реализация <see cref="IJsonConverter"/>
		/// </summary>
		/// <returns>Тип объекта, помеченного как реализация <see cref="IJsonConverter"/></returns>
		public static async Task<Type?> GetConverterType()
		{
			IEnumerable<Type> Implementations = await GetImplementations();

			foreach (var Type in Implementations)
			{
				return Type;
			}

			return null;
		}

		private static async Task<IEnumerable<Type>> GetImplementations()
		{
			if (_Implementations != null)
			{
				return _Implementations;
			}

			_Implementations = await Helper.GetTypesWithAttribute<JsonConverterImplementationAttribute>();

			return _Implementations;
		}
	}
}
