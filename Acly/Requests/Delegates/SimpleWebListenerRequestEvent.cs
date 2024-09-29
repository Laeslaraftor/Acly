using System.Net;

namespace Acly.Requests
{
    /// <summary>
    /// Вызывается при получении веб запроса
    /// </summary>
    /// <param name="Listener">Прослушиватель веб запросов</param>
    /// <param name="Context">Запрос</param>
    public delegate void SimpleWebListenerRequestEvent(SimpleWebListener Listener, HttpListenerContext Context);
}
