using System;
using System.Threading.Tasks;

namespace Acly
{
    /// <summary>
    /// Интерфейс планировщика
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Выполнить действие
        /// </summary>
        /// <param name="action">Действие, которое необходимо выполнить</param>
        public void Dispatch(Action action);
        /// <summary>
        /// Выполнить действие асинхронно
        /// </summary>
        /// <param name="action">Действие, которое необходимо выполнить</param>
        public Task DispatchAsync(Action action);
    }
}
