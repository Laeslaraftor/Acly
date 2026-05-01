using System;
using System.Net;

namespace Acly.Requests
{
    /// <summary>
    /// Вызывается при получении веб запроса
    /// </summary>
    /// <param name="listener">Прослушиватель веб запросов</param>
    /// <param name="context">Запрос</param>
    public delegate void SimpleWebListenerRequestEvent(SimpleWebListener listener, HttpListenerContext context);
    /// <summary>
    /// Вызывается при обработке исключения
    /// </summary>
    /// <param name="listener">Прослушиватель веб запросов</param>
    /// <param name="error">Обрабатываемое исключение</param>
    public delegate void SimpleWebListenerExceptionEvent(SimpleWebListener listener, Exception error);
}
