using System.Runtime.Serialization;

namespace Acly.Requests
{
	/// <summary>
	/// Статус ответа Api сервера
	/// </summary>
	public enum ApiResponseStatus
	{
		/// <summary>
		/// Ответ с ошибкой
		/// </summary>
		[EnumMember(Value = "error")]
		Error,
		/// <summary>
		/// Успешный ответ
		/// </summary>
		[EnumMember(Value = "success")]
		Success
	}
}
