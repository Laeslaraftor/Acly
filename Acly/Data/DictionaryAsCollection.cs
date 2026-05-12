using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Acly
{
    /// <summary>
    /// Словарь в виде коллекции
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TValue">Тип значения</typeparam>
    public partial class DictionaryAsCollection<TKey, TValue> 
        : Disposable, IDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>, INotifyCollectionChanged
    {
        /// <summary>
        /// Создать новый экземпляр словаря
        /// </summary>
        public DictionaryAsCollection() 
            : this([])
        {
        }
        /// <summary>
        /// Создать новый экземпляр словаря
        /// </summary>
        public DictionaryAsCollection(IEnumerable<KeyValuePair<TKey, TValue>> values)
            : this([.. values])
        {
        }
        private DictionaryAsCollection(EditableCollection<KeyValuePair<TKey, TValue>> list)
        {
            list.CollectionChanged += OnCollectionChanged;
            list.PropertyChanged += OnPropertyChanged;
            list.PropertyChanging += OnPropertyChanging;
            _list = list;
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
        public KeyValuePair<TKey, TValue> this[int index]
        {
            get => _list[index];
            set
            {
                var currentValue = _list[index];

                if (!Equals(currentValue.Key, value.Key))
                {
                    CheckKey(value.Key);
                }

                _list[index] = value;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        /// <exception cref="ArgumentException"></exception>
        public TValue this[TKey key]
        {
            get => _list.First(pair => Equals(pair.Key, key)).Value;
            set
            {
                for (int i = 0; i < _list.Count; i++)
                {
                    var pair = _list[i];

                    if (Equals(pair.Key, key))
                    {
                        pair = new(key, value);
                        _list[i] = pair;

                        return;
                    }
                }

                throw new ArgumentException("Значения с таким ключом нет в словаре", nameof(value));
            }
        }
        /// <summary>
        /// <inheritdoc cref="IDictionary{TKey, TValue}.Keys"/>
        /// </summary>
        public ElementCollection<TKey> Keys
        {
            get
            {
                if (IsDisposed)
                {
                    throw new InvalidOperationException(ObjectDisposedExceptionMessage);
                }

                _keys ??= new(_list);
                return _keys;
            }
        }
        /// <summary>
        /// <inheritdoc cref="IDictionary{TKey, TValue}.Values"/>
        /// </summary>
        public ElementCollection<TValue> Values
        {
            get
            {
                if (IsDisposed)
                {
                    throw new InvalidOperationException(ObjectDisposedExceptionMessage);
                }

                _values ??= new(_list);
                return _values;
            }
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _list.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly => false;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        private readonly EditableCollection<KeyValuePair<TKey, TValue>> _list;
        private KeyCollection? _keys;
        private ValueCollection? _values;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="item"><inheritdoc/></param>
        public void Insert(int index, KeyValuePair<TKey, TValue> item)
        {
            CheckKey(item.Key);
            _list.Insert(index, item);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        public void Add(TKey key, TValue value)
        {
            Add(new(key, value));
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            CheckKey(item.Key);
            _list.Add(item);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(TKey key)
        {
            var index = IndexOf(key);

            if (index == -1)
            {
                return false;
            }

            _list.RemoveAt(index);

            return true;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _list.Remove(item);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        public void RemoveAt(int index) => _list.RemoveAt(index);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear() => _list.Clear();

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            if (_keys != null)
            {
                _keys.Dispose();
                _keys = null;
            }
            if (_values != null)
            {
                _values.Dispose();
                _values = null;
            }

            _list.CollectionChanged -= OnCollectionChanged;
            _list.PropertyChanged -= OnPropertyChanged;
            _list.PropertyChanging -= OnPropertyChanging;
        }

        private void CheckKey(TKey key)
        {
            if (ContainsKey(key))
            {
                throw new ArgumentException("Значение с таким ключом уже существует", nameof(key));
            }
        }

        #endregion

        #region Дополнительно

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item) => _list.Contains(item);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool ContainsKey(TKey key)
        {
            return _list.Any(pair => Equals(pair.Key, key));
        }
#pragma warning disable CS8767 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует неявно реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value)
#pragma warning restore CS8767 // Допустимость значений NULL для ссылочных типов в типе параметра не соответствует неявно реализованному элементу (возможно, из-за атрибутов допустимости значений NULL).
        {
            value = default;

            foreach (var pair in _list)
            {
                if (Equals(pair.Key, key))
                {
                    value = pair.Value!;
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="arrayIndex"><inheritdoc/></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int IndexOf(KeyValuePair<TKey, TValue> item) => _list.IndexOf(item);
        /// <summary>
        /// <inheritdoc cref="IList{TKey}.IndexOf(TKey)"/>
        /// </summary>
        /// <param name="key"><inheritdoc cref="IList{TKey}.IndexOf(TKey)"/></param>
        /// <returns><inheritdoc cref="IList{TKey}.IndexOf(TKey)"/></returns>
        public int IndexOf(TKey key) => Keys.IndexOf(key);
        /// <summary>
        /// <inheritdoc cref="IList{TValue}.IndexOf(TValue)"/>
        /// </summary>
        /// <param name="value"><inheritdoc cref="IList{TValue}.IndexOf(TValue)"/></param>
        /// <returns><inheritdoc cref="IList{TValue}.IndexOf(TValue)"/></returns>
        public int IndexOf(TValue value) => Values.IndexOf(value);

        #endregion   

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region События

        /// <summary>
        /// Событие изменения коллекции
        /// </summary>
        /// <param name="e">Аргументы события</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnCollectionChanged(e);
        }
        private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            OnPropertyChanging(e);
        }
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged(e);
        }

        #endregion

        #region Классы

        /// <summary>
        /// Базовый класс коллекции элементов словаря, доступной только для чтения
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public abstract class ElementCollection<T> : Disposable, IList<T>, IReadOnlyList<T>, INotifyCollectionChanged
        {
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public event NotifyCollectionChangedEventHandler? CollectionChanged;

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public abstract int Count { get; }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="index"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            /// <exception cref="InvalidOperationException"></exception>
            public T this[int index]
            {
                get => GetElementAt(index);
                set => throw new InvalidOperationException(ReadOnlyException);
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            public bool IsReadOnly => true;

            #region Управление

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="item"><inheritdoc/></param>
            /// <exception cref="InvalidOperationException"></exception>
            public void Add(T item)
            {
                throw new InvalidOperationException(ReadOnlyException);
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <exception cref="InvalidOperationException"></exception>
            public void Clear()
            {
                throw new InvalidOperationException(ReadOnlyException);
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="index"><inheritdoc/></param>
            /// <param name="item"><inheritdoc/></param>
            /// <exception cref="InvalidOperationException"></exception>
            public void Insert(int index, T item)
            {
                throw new InvalidOperationException(ReadOnlyException);
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="item"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            /// <exception cref="InvalidOperationException"></exception>
            public bool Remove(T item)
            {
                throw new InvalidOperationException(ReadOnlyException);
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="index"><inheritdoc/></param>
            /// <exception cref="InvalidOperationException"></exception>
            public void RemoveAt(int index)
            {
                throw new InvalidOperationException(ReadOnlyException);
            }

            /// <summary>
            /// Получить элемент по его индексу
            /// </summary>
            /// <param name="index">Индекс элемента, которого надо получить</param>
            /// <returns>Объект по указанному индексу</returns>
            protected abstract T GetElementAt(int index);

            #endregion

            #region Дополнительно

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="item"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public int IndexOf(T item)
            {
                int index = 0;

                foreach (var value in this)
                {
                    if (Equals(value, item))
                    {
                        return index;
                    }

                    index++;
                }

                return -1;
            }
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="item"><inheritdoc/></param>
            /// <returns><inheritdoc/></returns>
            public bool Contains(T item) => IndexOf(item) != -1;
            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <param name="array"><inheritdoc/></param>
            /// <param name="arrayIndex"><inheritdoc/></param>
            public void CopyTo(T[] array, int arrayIndex)
            {
                for (int i = arrayIndex; i < Math.Min(array.Length, Count); i++)
                {
                    array[i] = this[i];
                }
            }

            #endregion

            #region Перечисление

            /// <summary>
            /// <inheritdoc/>
            /// </summary>
            /// <returns><inheritdoc/></returns>
            public abstract IEnumerator<T> GetEnumerator();
            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion

            #region События

            /// <summary>
            /// Событие изменения коллекции
            /// </summary>
            /// <param name="e">Аргументы события</param>
            protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
            {
                CollectionChanged?.Invoke(this, e);
            }

            #endregion

            #region Константы

            private const string ReadOnlyException = "Невозможно изменить коллекцию, доступную только для чтения";

            #endregion
        }
        private sealed class KeyCollection : ElementCollection<TKey>
        {
            public KeyCollection(EditableCollection<KeyValuePair<TKey, TValue>> list)
            {
                _list = list;
                list.CollectionChanged += OnCollectionChanged;
                list.PropertyChanged += OnPropertyChanged;
                list.PropertyChanging += OnPropertyChanging;
            }

            public override int Count => _list.Count;

            private readonly EditableCollection<KeyValuePair<TKey, TValue>> _list;

            #region Управление

            protected override TKey GetElementAt(int index) => _list[index].Key;

            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);

                _list.CollectionChanged -= OnCollectionChanged;
                _list.PropertyChanged -= OnPropertyChanged;
                _list.PropertyChanging -= OnPropertyChanging;
            }

            #endregion

            #region Перечисление

            public override IEnumerator<TKey> GetEnumerator()
            {
                foreach (var pair in _list)
                {
                    yield return pair.Key;
                }
            }

            #endregion

            #region События

            private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                OnPropertyChanging(e);
            }
            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e);
            }
            private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                OnCollectionChanged(e);
            }

            #endregion
        }
        private sealed class ValueCollection : ElementCollection<TValue>
        {
            public ValueCollection(EditableCollection<KeyValuePair<TKey, TValue>> list)
            {
                _list = list;
                list.CollectionChanged += OnCollectionChanged;
                list.PropertyChanged += OnPropertyChanged;
                list.PropertyChanging += OnPropertyChanging;
            }

            public override int Count => _list.Count;

            private readonly EditableCollection<KeyValuePair<TKey, TValue>> _list;

            #region Управление

            protected override TValue GetElementAt(int index) => _list[index].Value;

            protected override void Dispose(bool isDisposing)
            {
                base.Dispose(isDisposing);

                _list.CollectionChanged -= OnCollectionChanged;
                _list.PropertyChanged -= OnPropertyChanged;
                _list.PropertyChanging -= OnPropertyChanging;
            }

            #endregion

            #region Перечисление

            public override IEnumerator<TValue> GetEnumerator()
            {
                foreach (var pair in _list)
                {
                    yield return pair.Value;
                }
            }

            #endregion

            #region События

            private void OnPropertyChanging(object sender, PropertyChangingEventArgs e)
            {
                OnPropertyChanging(e);
            }
            private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                OnPropertyChanged(e);
            }
            private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                OnCollectionChanged(e);
            }

            #endregion
        }

        #endregion
    }
}
