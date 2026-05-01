using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;

namespace Acly
{
    /// <summary>
    /// Записная книжка с отслеживаемыми изменениями
    /// </summary>
    /// <typeparam name="TKey">Тип данных ключа</typeparam>
    /// <typeparam name="TValue">Тип данных значение</typeparam>
    [Serializable]
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, IDeserializationCallback
    {
        /// <summary>
        /// Создать новый экземпляр записной книжки с отслеживаемыми изменениями
        /// </summary>
        public ObservableDictionary()
        {
            _dictionary = [];
        }
        /// <summary>
        /// Создать новый экземпляр записной книжки с отслеживаемыми изменениями
        /// </summary>
        /// <param name="Dictionary">Основа</param>
        public ObservableDictionary(IDictionary<TKey, TValue> Dictionary)
        {
            _dictionary = new Dictionary<TKey, TValue>(Dictionary);
        }
        /// <summary>
        /// Создать новый экземпляр записной книжки с отслеживаемыми изменениями
        /// </summary>
        /// <param name="Collection">Основа</param>
        public ObservableDictionary(ICollection<KeyValuePair<TKey, TValue>> Collection)
        {
            _dictionary = new Dictionary<TKey, TValue>(Collection);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<TKey> Keys => _dictionary.Keys;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<TValue> Values => _dictionary.Values;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _dictionary.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly { get; }

        private readonly Dictionary<TKey, TValue> _dictionary;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            OnCollectionChanged();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dictionary.Add(item.Key, item.Value);
            OnCollectionChanged();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(TKey key)
        {
            bool result = _dictionary.Remove(key);
            OnCollectionChanged();

            return result;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool Result = _dictionary.Remove(item.Key);
            OnCollectionChanged();

            return Result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
            OnCollectionChanged();
        }

        #endregion

        #region Получение

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="arrayIndex"><inheritdoc/></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary)_dictionary).CopyTo(array, arrayIndex);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        #endregion

        #region Перечисления

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<KeyValuePair<TKey, TValue>>(_dictionary).GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new List<KeyValuePair<TKey, TValue>>(_dictionary).GetEnumerator();
        }

        #endregion

        #region События

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="sender"><inheritdoc/></param>
        public virtual void OnDeserialization(object sender) => _dictionary.OnDeserialization(sender);

        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}
