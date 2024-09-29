using System;
using System.Collections.Generic;

namespace Acly
{
	public static partial class Helper
	{
		/// <summary>
		/// Конвертировать записную книжку только для чтения в обычную
		/// </summary>
		/// <typeparam name="TKey">Тип данных ключа</typeparam>
		/// <typeparam name="TValue">Тип данных значения</typeparam>
		/// <param name="Original">Записная книжка только для чтения</param>
		/// <returns>Записная книжка</returns>
		/// <exception cref="ArgumentNullException">Записная книжка не указана</exception>
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> Original)
		{
			if (Original == null)
			{
				throw new ArgumentNullException(nameof(Original), "Записная книжка не указана");
			}

			Dictionary<TKey, TValue> Result = new();

			foreach (var Key in Original.Keys)
			{
				Result.Add(Key, Original[Key]);
			}

			return Result;
		}
	}
}
