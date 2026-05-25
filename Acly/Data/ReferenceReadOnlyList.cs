using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Acly
{
    /// <summary>
    /// Класс списка-ссылки только для чтения оригинального списка
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    public class ReferenceReadOnlyList<T> : Disposable, ICollection<T>, IReadOnlyList<T>, IList<T>, INotifyCollectionChanged
    {
        /// <summary>
        /// Создать экземпляр класса списка-ссылки только для чтения оригинального списка
        /// </summary>
        /// <param name="reference">Основная коллекция</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReferenceReadOnlyList(IList<T> reference)
        {
            _reference = reference ?? throw new ArgumentNullException(nameof(reference));

            if (reference is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged += OnReferenceCollectionChanged;
            }
            if (reference is INotifyPropertyChanged notifyProperty)
            {
                notifyProperty.PropertyChanged += OnNotifyPropertyPropertyChanged;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public T this[int index] => _reference[index];
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _reference.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly => true;

        T IList<T>.this[int index]
        {
            get => _reference[index];
            set => throw new InvalidOperationException(ReadOnlyCollectionInvalidExceptionMessage);
        }

        private readonly IList<T> _reference;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int IndexOf(T item)
        {
            return _reference.IndexOf(item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(T item)
        {
            return _reference.Contains(item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="arrayIndex"><inheritdoc/></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _reference.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (_reference is INotifyCollectionChanged notifyCollection)
            {
                notifyCollection.CollectionChanged -= OnReferenceCollectionChanged;
            }
            if (_reference is INotifyPropertyChanged notifyProperty)
            {
                notifyProperty.PropertyChanged -= OnNotifyPropertyPropertyChanged;
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw new InvalidOperationException(ReadOnlyCollectionInvalidExceptionMessage);
        }
        void ICollection<T>.Clear()
        {
            throw new InvalidOperationException(ReadOnlyCollectionInvalidExceptionMessage);
        }
        bool ICollection<T>.Remove(T item)
        {
            throw new InvalidOperationException(ReadOnlyCollectionInvalidExceptionMessage);
        }
        void IList<T>.Insert(int index, T item)
        {
            throw new InvalidOperationException(ReadOnlyCollectionInvalidExceptionMessage);
        }
        void IList<T>.RemoveAt(int index)
        {
            throw new InvalidOperationException(ReadOnlyCollectionInvalidExceptionMessage);
        }

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _reference.GetEnumerator();
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
            OnPropertyChanged(e);
        }

        #endregion

        #region Константы

        private const string ReadOnlyCollectionInvalidExceptionMessage = "Невозможно изменить коллекцию, так как она доступна только для чтения!";

        #endregion
    }
}
