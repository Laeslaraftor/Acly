using System;
using System.Collections;
using System.Reflection;

namespace Acly
{
    /// <summary>
    /// Список, основанный на рефлексии
    /// </summary>
#pragma warning disable CA1010 // Также необходимо реализовать универсальный интерфейс
    public class ReflectionList : Disposable, IList, IEnumerable
#pragma warning restore CA1010 // Также необходимо реализовать универсальный интерфейс
    {
        /// <summary>
        /// Создать новый экземпляр списка
        /// </summary>
        /// <param name="obj">Список</param>
        /// <exception cref="ArgumentException"></exception>
        public ReflectionList(object obj)
            : this(obj as IEnumerable ?? throw new ArgumentException($"Объект {obj} не является перечисляемым!", nameof(obj)))
        {
        }
        /// <summary>
        /// Создать новый экземпляр списка
        /// </summary>
        /// <param name="list">Список</param>
        public ReflectionList(IEnumerable list)
        {
            Enumerable = list ?? throw new ArgumentNullException(nameof(list));

            Type listType = list.GetType();

            if (listType.TryFindMethod(nameof(Add), out var addMethod) &&
                listType.TryFindMethod(nameof(Insert), out var insertMethod) &&
                listType.TryFindMethod(nameof(RemoveAt), out var removeAtMethod) &&
                listType.TryFindMethod(nameof(Remove), out var removeMethod) &&
                listType.TryFindMethod(nameof(Clear), out var clearMethod) &&
                listType.TryFindProperty(nameof(Count), out var countProperty) &&
                listType.TryFindProperty("Item", out var itemProperty))
            {
                _addMethod = addMethod;
                _insertMethod = insertMethod;
                _removeAtMethod = removeAtMethod;
                _removeMethod = removeMethod;
                _clearMethod = clearMethod;
                _countProperty = countProperty;
                _itemProperty = itemProperty;
            }
            else
            {
                throw new ArgumentException($"Объект не является списком!", nameof(list));
            }
        }

        /// <summary>
        /// Конвертер значений. Используется для обработки входящих значений.
        /// </summary>
        public ConvertCollectionValue? Converter
        {
            get => field;
            set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(Converter));
                    field = value;
                    OnPropertyChanged(nameof(Converter));
                }
            }
        }
        /// <summary>
        /// Список объектов
        /// </summary>
        public IEnumerable Enumerable { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => (int)_countProperty.GetValue(Enumerable);
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsFixedSize => false;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsReadOnly => false;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool IsSynchronized => false;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public object? SyncRoot => null;
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public object this[int index]
        {
            get
            {
                _methodParams[0] = index;
                return _itemProperty.GetValue(Enumerable, _methodParams);
            }
            set
            {
                _methodParams[0] = index;
                _itemProperty.SetValue(Enumerable, Convert(value, index), _methodParams);
            }
        }

        private readonly MethodInfo _removeAtMethod;
        private readonly MethodInfo _removeMethod;
        private readonly MethodInfo _insertMethod;
        private readonly MethodInfo _addMethod;
        private readonly MethodInfo _clearMethod;
        private readonly PropertyInfo _countProperty;
        private readonly PropertyInfo _itemProperty;
        private readonly object?[] _methodParams = new object?[1];
        private readonly object?[] _methodParams2 = new object?[2];

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(object? value)
        {
            return IndexOf(value) != -1;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int IndexOf(object? value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (value?.Equals(this[i]) == true)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int Add(object? item)
        {
            int startCount = Count;

            _methodParams[0] = Convert(item, startCount);
            _addMethod.Invoke(Enumerable, _methodParams);

            int endCount = Count;

            if (startCount == endCount)
            {
                return -1;
            }

            return 1;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="item"><inheritdoc/></param>
        public void Insert(int index, object? item)
        {
            _methodParams2[0] = index;
            _methodParams2[1] = Convert(item, index);
            _insertMethod.Invoke(Enumerable, _methodParams2);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear()
        {
            _clearMethod.Invoke(Enumerable, []);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="index"><inheritdoc/></param>
        public void RemoveAt(int index)
        {
            _methodParams[0] = index;
            _removeAtMethod.Invoke(Enumerable, _methodParams);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"><inheritdoc/></param>
        public void Remove(object? value)
        {
            _methodParams[0] = value;
            _removeMethod.Invoke(Enumerable, _methodParams);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="array"><inheritdoc/></param>
        /// <param name="index"><inheritdoc/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            for (int i = 0; i < Count; i++)
            {
                array.SetValue(this[i], i + index);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            _methodParams[0] = null;
            _methodParams2[0] = null;
            _methodParams2[1] = null;
        }

        private object? Convert(object? value, int index)
        {
            if (Converter != null)
            {
                value = Converter(value, index, this);
            }

            return value;
        }

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator GetEnumerator()
        {
            return Enumerable.GetEnumerator();
        }


        #endregion
    }
}
