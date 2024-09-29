using Acly.Platforms;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Acly.Player.Implementations
{
	/// <summary>
	/// Класс для поиска реализаций <see cref="ISimplePlayer"/>
	/// </summary>
	public static class SimplePlayerImplementations
	{
		private static IEnumerable<Type>? _Implementations;

		/// <summary>
		/// Получить реализацию SimplePlayer для указанной платформы
		/// </summary>
		/// <param name="Platform">Платформа</param>
		/// <returns>Тип с реализацией SimplePlayer</returns>
		public static async Task<Type?> GetPlatformImplementation(RuntimePlatform Platform)
		{
			IEnumerable<Type> Implementations = await GetImplementations();
			Type? Result = null;

			foreach (var Type in Implementations)
			{
				foreach (var Attribute in Type.GetCustomAttributes<SimplePlayerImplementationAttribute>())
				{
					if (Attribute.Platform.HasFlag(Platform))
					{
						Result = Type;
						break;
					}
				}
			}

			if (Result?.IsImplementsInterface<ISimplePlayer>() == false)
			{
				throw new SimplePlayerImplementationException(Result);
			}

			return Result;
		}
		/// <summary>
		/// Получить реализацию SimplePlayer для текущей платформы
		/// </summary>
		/// <returns>Реализация SimplePlayer для текущей платформы</returns>
		public static async Task<Type?> GetCurrentPlatformImplementation()
		{
			return await GetPlatformImplementation(Platform.Current);
		}

		private static async Task<IEnumerable<Type>> GetImplementations()
		{
			if (_Implementations != null)
			{
				return _Implementations;
			}

			_Implementations = await Helper.GetTypesWithAttribute<SimplePlayerImplementationAttribute>();

			return _Implementations;
		}
	}
}
