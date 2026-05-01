using Acly.Tasks;

namespace Acly.Performing
{
    /// <summary>
    /// Событие загрузки
    /// </summary>
    /// <param name="loading">Загрузка, вызвавшая событие</param>
    /// <param name="error">Возникшая ошибка</param>
    public delegate void LoadingFailEvent(ILoading loading, IAsyncTaskError error);
}
