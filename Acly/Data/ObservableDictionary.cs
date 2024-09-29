using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            _Dictionary = new Dictionary<TKey, TValue>();
        }
        /// <summary>
        /// Создать новый экземпляр записной книжки с отслеживаемыми изменениями
        /// </summary>
        /// <param name="Dictionary">Основа</param>
        public ObservableDictionary(IDictionary<TKey, TValue> Dictionary)
        {
            _Dictionary = new Dictionary<TKey, TValue>(Dictionary);
        }
        /// <summary>
        /// Создать новый экземпляр записной книжки с отслеживаемыми изменениями
        /// </summary>
        /// <param name="Collection">Основа</param>
        public ObservableDictionary(ICollection<KeyValuePair<TKey, TValue>> Collection)
        {
            _Dictionary = new Dictionary<TKey, TValue>(Collection);
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
            get => _Dictionary[key];
            set => _Dictionary[key] = value;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<TKey> Keys => new Collection<TKey>(_Dictionary.Keys.ToList());
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<TValue> Values => new Collection<TValue>(_Dictionary.Values.ToList());
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _Dictionary.Count;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly { get; }

        private readonly Dictionary<TKey, TValue> _Dictionary;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        public void Add(TKey key, TValue value)
        {
            _Dictionary.Add(key, value);
            OnCollectionChanged();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _Dictionary.Add(item.Key, item.Value);
            OnCollectionChanged();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(TKey key)
        {
            bool Result = _Dictionary.Remove(key);
            OnCollectionChanged();

            return Result;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            bool Result = _Dictionary.Remove(item.Key);
            OnCollectionChanged();

            return Result;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear()
        {
            _Dictionary.Clear();
            OnCollectionChanged();
        }

        #endregion

        #region Получение

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(KeyValuePair<TKey, TValue> item) => _Dictionary.Contains(item);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool ContainsKey(TKey key) => _Dictionary.ContainsKey(key);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="arrayIndex"><inheritdoc/></param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => ((IDictionary)_Dictionary).CopyTo(array, arrayIndex);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool TryGetValue(TKey key, out TValue value) => _Dictionary.TryGetValue(key, out value);

        #endregion

        #region Перечисления

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<KeyValuePair<TKey, TValue>>(_Dictionary).GetEnumerator();
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new List<KeyValuePair<TKey, TValue>>(_Dictionary).GetEnumerator();
        }

        #endregion

        #region События

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="sender"><inheritdoc/></param>
        public virtual void OnDeserialization(object sender) => _Dictionary.OnDeserialization(sender);

        private void OnCollectionChanged()
        {
            CollectionChanged?.Invoke(this, new(NotifyCollectionChangedAction.Reset));
        }

        #endregion
    }
}
