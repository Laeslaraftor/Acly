namespace Acly.Requests
{
    /// <summary>
    /// Интерфейс читателя
    /// </summary>
    public interface IListener
    {
        /// <summary>
        /// Вызывается когда <see cref="IListener"/> запускается или останавливается
        /// </summary>
        public event ListenerEvent? StartedStateChanged;

        /// <summary>
        /// Запущен ли <see cref="IListener"/>
        /// </summary>
        public bool IsStarted { get; }

        #region Управление

        /// <summary>
        /// Запустить <see cref="IListener"/>
        /// </summary>
        /// <returns>
        /// Был ли запущен <see cref="IListener"/>
        /// </returns>
        public bool Start();
        /// <summary>
        /// Остановить <see cref="IListener"/>
        /// </summary>
        ///  <returns>
        /// Был ли остановлен <see cref="IListener"/>
        /// </returns>
        public bool Stop();

        #endregion
    }
}
