using System;
using System.Net;

namespace Acly.Requests
{
	public partial class Ajax
	{
		private class Client : WebClient
		{
			public Client(Timeout Timeout)
			{
				if (Timeout == null)
				{
					throw new ArgumentNullException(nameof(Timeout), "Время ожидания не указано");
				}

				_Timeout = Timeout;
			}

			private Timeout _Timeout;

			/// <summary>
			/// Получить <see cref="WebRequest"/>
			/// </summary>
			/// <param name="address">Адрес запроса</param>
			/// <returns>Веб запрос</returns>
			protected override WebRequest GetWebRequest(Uri address)
			{
				WebRequest Request = base.GetWebRequest(address);
				Request.Timeout = _Timeout.Milliseconds;

				return Request;
			}
		}
	}
}
