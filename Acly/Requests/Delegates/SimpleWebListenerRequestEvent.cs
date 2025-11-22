using System;
using System.Net;

namespace Acly.Requests
{
    /// <summary>
    /// Вызывается при получении веб запроса
    /// </summary>
    /// <param name="Listener">Прослушиватель веб запросов</param>
    /// <param name="Context">Запрос</param>
    public delegate void SimpleWebListenerRequestEvent(SimpleWebListener Listener, HttpListenerContext Context);
    /// <summary>
    /// Вызывается при обработке исключения
    /// </summary>
    /// <param name="Listener">Прослушиватель веб запросов</param>
    /// <param name="Error">Обрабатываемое исключение</param>
    public delegate void SimpleWebListenerExceptionEvent(SimpleWebListener Listener, Exception Error);
}
