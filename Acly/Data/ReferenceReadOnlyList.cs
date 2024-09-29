using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System;

namespace Acly
{
    /// <summary>
    /// Класс списка-ссылки только для чтения оригинального списка
    /// </summary>
    /// <typeparam name="T">Тип данных списка</typeparam>
    public class ReferenceReadOnlyList<T> : IReadOnlyList<T>, INotifyCollectionChanged
    {
        /// <summary>
        /// Создать экземпляр класса списка-ссылки только для чтения оригинального списка
        /// </summary>
        /// <param name="Reference">Основная коллекция</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReferenceReadOnlyList(ObservableList<T> Reference)
        {
            if (Reference == null)
            {
                throw new ArgumentNullException(nameof(Reference));
            }

            _Reference = Reference;
            Reference.CollectionChanged += OnReferenceCollectionChanged;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public T this[int index] => _Reference[index];
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _Reference.Count;

        private readonly ObservableList<T> _Reference;

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new List<T>(_Reference).GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<T>(_Reference).GetEnumerator();
        }

        #endregion

        #region События

        private void OnReferenceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        #endregion
    }
}
