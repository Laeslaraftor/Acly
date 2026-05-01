using System;

namespace Acly
{
    /// <summary>
    /// Базовый класс очищаемого объекта
    /// </summary>
    [Serializable]
    public abstract class Disposable : ObservableObject, IDisposable
    {
        /// <summary>
        /// Очистка объекта
        /// </summary>
        ~Disposable()
        {
            Dispose(false);
        }

        /// <summary>
        /// Событие, которое вызывается при очистке экземпляра
        /// </summary>
        public event EventHandler? Disposed;

        /// <summary>
        /// Очищен ли объект
        /// </summary>
        public bool IsDisposed
        {
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(IsDisposed));
                    field = value;
                    OnPropertyChanged(nameof(IsDisposed));
                }
            }
        }

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
#pragma warning disable CA1063
        public void Dispose()
#pragma warning restore CA1063
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            Dispatch(Disposed);
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Очистка объекта
        /// </summary>
        /// <param name="isDisposing">Ручная ли очистка</param>
        protected virtual void Dispose(bool isDisposing)
        {
        }

        #endregion
    }
}
