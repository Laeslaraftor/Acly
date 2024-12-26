using Acly.Tasks;

namespace Acly.Performing
{
    /// <summary>
    /// Событие загрузки
    /// </summary>
    /// <param name="Loading">Загрузка, вызвавшая событие</param>
    /// <param name="Error">Возникшая ошибка</param>
    public delegate void LoadingFailEvent(ILoading Loading, IAsyncTaskError Error);
}
