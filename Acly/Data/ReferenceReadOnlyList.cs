using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;
using System.ComponentModel;

namespace Acly
{
    /// <summary>
    /// Класс списка-ссылки только для чтения оригинального списка
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    public class ReferenceReadOnlyList<T> : IReadOnlyList<T>, INotifyPropertyChanged, INotifyCollectionChanged, IDisposable
    {
        /// <summary>
        /// Создать экземпляр класса списка-ссылки только для чтения оригинального списка
        /// </summary>
        /// <param name="Reference">Основная коллекция</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReferenceReadOnlyList(IList<T> Reference)
        {
            if (Reference == null)
            {
                throw new ArgumentNullException(nameof(Reference));
            }

            _Reference = Reference;

            if (Reference is INotifyCollectionChanged NotifyCollection)
            {
                NotifyCollection.CollectionChanged += OnReferenceCollectionChanged;
            }
            if (Reference is INotifyPropertyChanged NotifyProperty)
            {
                NotifyProperty.PropertyChanged += OnNotifyPropertyPropertyChanged;
            }
        }
        /// <summary>
        /// Очистить объект
        /// </summary>
        ~ReferenceReadOnlyList()
        {
            Dispose(false);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public T this[int index] => _Reference[index];
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _Reference.Count;

        private readonly IList<T> _Reference;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Очистить объект
        /// </summary>
        /// <param name="IsDisposing">Ручная очистка</param>
        protected virtual void Dispose(bool IsDisposing)
        {
            if (_Reference is INotifyCollectionChanged NotifyCollection)
            {
                NotifyCollection.CollectionChanged -= OnReferenceCollectionChanged;
            }
            if (_Reference is INotifyPropertyChanged NotifyProperty)
            {
                NotifyProperty.PropertyChanged -= OnNotifyPropertyPropertyChanged;
            }
        }

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _Reference.GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region События

        private void OnReferenceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }
        private void OnNotifyPropertyPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }

        #endregion
    }
}
