using Acly.Platforms;
using System;

namespace Acly.Player
{
	/// <summary>
	/// Исключение, вызывающееся при отсутствии реализаций <see cref="ISimplePlayer"/>
	/// </summary>
	public sealed class SimplePlayerNoImplementation : Exception
	{
		/// <summary>
		/// Вызвать исключение отсутствия реализаций
		/// </summary>
		/// <param name="Platform">Требуемая платформа</param>
		public SimplePlayerNoImplementation(RuntimePlatform Platform) : base(string.Format(_Message, Platform))
		{
		}

		private const string _Message = "Для платформы {0} отсутствует реализация";
	}
}
