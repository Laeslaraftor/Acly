using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Acly
{
    /// <summary>
    /// Класс книжки-ссылки только для чтения оригинальной книжки
    /// </summary>
    /// <typeparam name="TKey">Тип данных ключа</typeparam>
    /// <typeparam name="TValue">Тип данных значения</typeparam>
    public class ReferenceReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, INotifyCollectionChanged, IDeserializationCallback
    {
        /// <summary>
        /// Создать новый экземпляр класса книжки-ссылки только для чтения оригинальной книжки
        /// </summary>
        /// <param name="reference">Основная книжка</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReferenceReadOnlyDictionary(ObservableDictionary<TKey, TValue> reference)
        {
            _reference = reference ?? throw new ArgumentNullException(nameof(reference));
            reference.CollectionChanged += OnReferenceCollectionChanged;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event NotifyCollectionChangedEventHandler? CollectionChanged;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TValue this[TKey key] => _reference[key];
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<TKey> Keys => _reference.Keys;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ICollection<TValue> Values => _reference.Values;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _reference.Count;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        private readonly ObservableDictionary<TKey, TValue> _reference;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool ContainsKey(TKey key) => _reference.ContainsKey(key);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool TryGetValue(TKey key, out TValue value) => _reference.TryGetValue(key, out value);

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _reference.GetEnumerator();
        }
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

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="sender"><inheritdoc/></param>
        public virtual void OnDeserialization(object sender) => _reference.OnDeserialization(sender);

        #endregion
    }
}
