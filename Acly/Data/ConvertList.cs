using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Acly
{
    /// <summary>
    /// Список с конвертируемыми значениями
    /// </summary>
    /// <typeparam name="T">Изначальный тип</typeparam>
    /// <typeparam name="TCast">Конечный тип</typeparam>
    public class ConvertList<T, TCast> : Disposable, IList<TCast>, INotifyCollectionChanged
    {
        /// <summary>
        /// Создать новый экземпляр списка с конвертируемыми значениями
        /// </summary>
        /// <param name="list">Изначальный список</param>
        public ConvertList(IList<T> list)
            : this(list, DefaultConverter.Insntance)
        {
        }
        /// <summary>
        /// Создать новый экземпляр списка с конвертируемыми значениями
        /// </summary>
        /// <param name="list">Изначальный список</param>
        /// <param name="converter">Конвертер значений</param>
        public ConvertList(IList<T> list, IValueConverter<T, TCast> converter)
        {
            _converter = converter;
            _list = list;

            if (list is INotifyCollectionChanged observableCollection)
            {
                observableCollection.CollectionChanged += OnCollectionChanged;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public TCast this[int index]
        {
            get => _converter.Convert(_list[index]);
            set => _list[index] = _converter.ConvertBack(value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _list.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly => _list.IsReadOnly;

        private readonly IValueConverter<T, TCast> _converter;
        private readonly IList<T> _list;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(TCast item)
        {
            return _list.Contains(_converter.ConvertBack(item));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int IndexOf(TCast item)
        {
            return _list.IndexOf(_converter.ConvertBack(item));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="item"><inheritdoc/></param>
        public void Insert(int index, TCast item)
        {
            _list.Insert(index, _converter.ConvertBack(item));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public void Add(TCast item)
        {
            _list.Add(_converter.ConvertBack(item));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear() => _list.Clear();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(TCast item)
        {
            return _list.Remove(_converter.ConvertBack(item));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        public void RemoveAt(int index) => _list.RemoveAt(index);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="arrayIndex"><inheritdoc/></param>
        public void CopyTo(TCast[] array, int arrayIndex)
        {
            for (int i = 0; i < Math.Min(array.Length, Count); i++)
            {
                array[i] = this[i];
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (_list is INotifyCollectionChanged observableCollection)
            {
                observableCollection.CollectionChanged -= OnCollectionChanged;
            }
        }

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<TCast> GetEnumerator()
        {
            foreach (var item in _list)
            {
                yield return _converter.Convert(item);
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region События

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        #endregion

        #region Классы

        private class DefaultConverter : IValueConverter<T, TCast>
        {
            public TCast Convert(T value)
            {
                return (TCast)(object)value!;
            }
            public T ConvertBack(TCast value)
            {
                return (T)(object)value!;
            }

            public static readonly DefaultConverter Insntance = new();
        }

        #endregion
    }
}
