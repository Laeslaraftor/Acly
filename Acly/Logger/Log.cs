using Acly.Logger;
using System;
using System.Threading.Tasks;

namespace Acly
{
	/// <summary>
	/// Класс для отправки сообщений
	/// </summary>
	public static class Log
	{
		/// <summary>
		/// Если Logger выключен, то сообщения отправляться не будут
		/// </summary>
		public static bool Enabled { get; set; } = true;

		private static readonly ILogger _Default = new StandardLogger();
		private static ILogger? _Logger;

		#region Сообщения

		/// <summary>
		/// Отправить обычное сообщение
		/// </summary>
		/// <param name="Message">Сообщение</param>
		public static async void Message(string Message)
		{
			ILogger? Logger = await GetImplementation();
			Logger?.Message(Message);
		}
		/// <summary>
		/// Отправить объект как сообщение
		/// </summary>
		/// <param name="Object">Объект для сообщения</param>
		public static async void Message(object Object)
		{
			ILogger? Logger = await GetImplementation();
			Logger?.Message(Object);
		}

		/// <summary>
		/// Отправить предупреждение
		/// </summary>
		/// <param name="Message">Предупреждение</param>
		public static async void Warning(string Message)
		{
			ILogger? Logger = await GetImplementation();
			Logger?.Warning(Message);
		}
		/// <summary>
		/// Отправить объект как предупреждение
		/// </summary>
		/// <param name="Object">Объект для предупреждения</param>
		public static async void Warning(object Object)
		{
			ILogger? Logger = await GetImplementation();
			Logger?.Warning(Object);
		}

		/// <summary>
		/// Отправить сообщение об ошибке
		/// </summary>
		/// <param name="Message">Текст ошибки</param>
		public static async void Error(string Message)
		{
			ILogger? Logger = await GetImplementation();
			Logger?.Error(Message);
		}
		/// <summary>
		/// Отправить объект как сообщение об ошибке
		/// </summary>
		/// <param name="Object">Объект для сообщения об ошибке</param>
		public static async void Error(object Object)
		{
			ILogger? Logger = await GetImplementation();
			Logger?.Error(Object);
		}

		#endregion

		#region Поиск реализации

		private static async Task<ILogger?> GetImplementation()
		{
			if (!Enabled)
			{
				return null;
			}
			if (_Logger != null)
			{
				return _Logger;
			}

			Type? Implementation = await LoggerImplementations.GetImplementationType();

			if (Implementation == null)
			{
				return _Default;
			}
			if (!Implementation.IsImplementsInterface<ILogger>())
			{
				throw new LoggerImplementationException(Implementation);
			}

			_Logger = (ILogger)Activator.CreateInstance(Implementation);

			return _Logger;
		}

		#endregion
	}
}
