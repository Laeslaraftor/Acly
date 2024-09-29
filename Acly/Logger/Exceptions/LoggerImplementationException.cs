using System;

namespace Acly.Logger
{
	/// <summary>
	/// Исключение, вызываемое при неправильной реализации <see cref="ILogger"/>
	/// </summary>
	public sealed class LoggerImplementationException : ImplementationException
	{
		/// <summary>
		/// Вызвать исключение о неправильной реализации <see cref="ILogger"/>
		/// </summary>
		/// <param name="Type">Тип с неправильной реализацией</param>
#pragma warning disable CA1062
		public LoggerImplementationException(Type Type) : base(string.Format(_Message, Type.Name, "Logger", nameof(ILogger)))
#pragma warning restore CA1062
		{

		}

		private const string _Message = "Тип {0} был помечен как реализация {1}, но не реализует {2}";
	}
}
