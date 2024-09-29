using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acly.Logger
{
	/// <summary>
	/// Класс для поиска реализаций <see cref="ILogger"/>
	/// </summary>
	public static class LoggerImplementations
	{
		/// <summary>
		/// Получить список типов, помеченных как реализация <see cref="ILogger"/>
		/// </summary>
		/// <returns>Cписок типов, помеченных как реализация <see cref="ILogger"/></returns>
		public static async Task<Type?> GetImplementationType()
		{
			IEnumerable<Type> Types = await Helper.GetTypesWithAttribute<LoggerImplementation>();

			foreach (var Type in Types)
			{
				return Type;
			}

			return null;
		}
	}
}
