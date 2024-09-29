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
        /// <param name="Reference">Основная книжка</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReferenceReadOnlyDictionary(ObservableDictionary<TKey, TValue> Reference)
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
        public TValue this[TKey key] => _Reference[key];
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<TKey> Keys => new List<TKey>(_Reference.Keys);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<TValue> Values => new List<TValue>(_Reference.Values);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => _Reference.Count;

        private readonly ObservableDictionary<TKey, TValue> _Reference;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool ContainsKey(TKey key) => _Reference.ContainsKey(key);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="key"><inheritdoc/></param>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool TryGetValue(TKey key, out TValue value) => _Reference.TryGetValue(key, out value);

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new List<KeyValuePair<TKey, TValue>>(_Reference).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new List<KeyValuePair<TKey, TValue>>(_Reference).GetEnumerator();
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
        public virtual void OnDeserialization(object sender) => _Reference.OnDeserialization(sender);

        #endregion
    }
}
