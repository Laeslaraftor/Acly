namespace Acly.Requests
{
    /// <summary>
    /// Базовый класс, реализующий <see cref="IListener"/>
    /// </summary>
    public abstract class ListenerBase : IListener
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event ListenerEvent? StartedStateChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual bool IsStarted { get; private set; }

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public virtual bool Start()
        {
            if (IsStarted)
            {
                return false;
            }

            IsStarted = true;

            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public virtual bool Stop()
        {
            if (!IsStarted)
            {
                return false;
            }

            IsStarted = false;

            return true;
        }

        #endregion

        #region События

        /// <summary>
        /// Вызвать <see cref="StartedStateChanged"/>
        /// </summary>
        protected virtual void InvokeStartedStateChanged()
        {
            StartedStateChanged?.Invoke(this);
        }

        #endregion
    }
}
