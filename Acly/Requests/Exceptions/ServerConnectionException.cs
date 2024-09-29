using System;
using System.Runtime.Serialization;

namespace Acly.Requests.Exceptions
{
    /// <summary>
    /// Исключение, возникающее при неудачном подключении к серверу
    /// </summary>
    [Serializable]
    public class ServerConnectionException : Exception
	{
		/// <summary>
		/// Неудачное подключение к серверу
		/// </summary>
		public ServerConnectionException()
		{
		}
		/// <summary>
		/// Неудачное подключение к серверу
		/// </summary>
		/// <param name="message">Сообщение</param>
		public ServerConnectionException(string message) : base(message)
		{
		}
		/// <summary>
		/// Неудачное подключение к серверу
		/// </summary>
		/// <param name="message">Сообщение</param>
		/// <param name="innerException">Внутренняя ошибка</param>
		public ServerConnectionException(string message, Exception innerException) : base(message, innerException)
		{
		}
		/// <summary>
		/// Неудачное подключение к серверу
		/// </summary>
		/// <param name="info"><inheritdoc/></param>
		/// <param name="context"><inheritdoc/></param>
		protected ServerConnectionException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
