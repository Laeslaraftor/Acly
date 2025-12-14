using System;
using System.Collections;
using System.Reflection;

namespace Acly
{
    /// <summary>
    /// Список, основанный на рефлексии
    /// </summary>
#pragma warning disable CA1010 // Также необходимо реализовать универсальный интерфейс
    public class ReflectionList : IList, IEnumerable, IDisposable
#pragma warning restore CA1010 // Также необходимо реализовать универсальный интерфейс
    {
        /// <summary>
        /// Создать новый экземпляр списка
        /// </summary>
        /// <param name="Obj">Список</param>
        /// <exception cref="ArgumentException"></exception>
        public ReflectionList(object Obj)
            : this(Obj as IEnumerable ?? throw new ArgumentException($"Объект {Obj} не является перечисляемым!", nameof(Obj)))
        {
        }
        /// <summary>
        /// Создать новый экземпляр списка
        /// </summary>
        /// <param name="List">Список</param>
        public ReflectionList(IEnumerable List)
        {
            if (List == null)
            {
                throw new ArgumentNullException(nameof(List));
            }

            Enumerable = List;

            Type ListType = List.GetType();

            if (ListType.TryFindMethod(nameof(Add), out var AddMethod) &&
                ListType.TryFindMethod(nameof(Insert), out var InsertMethod) &&
                ListType.TryFindMethod(nameof(RemoveAt), out var RemoveAtMethod) &&
                ListType.TryFindMethod(nameof(Remove), out var RemoveMethod) &&
                ListType.TryFindMethod(nameof(Clear), out var ClearMethod) &&
                ListType.TryFindProperty(nameof(Count), out var CountProperty) &&
                ListType.TryFindProperty("Item", out var ItemProperty))
            {
                _AddMethod = AddMethod;
                _InsertMethod = InsertMethod;
                _RemoveAtMethod = RemoveAtMethod;
                _RemoveMethod = RemoveMethod;
                _ClearMethod = ClearMethod;
                _CountProperty = CountProperty;
                _ItemProperty = ItemProperty;
            }
            else
            {
                throw new ArgumentException($"Объект не является списком!", nameof(List));
            }
        }
        /// <summary>
        /// Очистить экземпляр
        /// </summary>
        ~ReflectionList()
        {
            Dispose(false);
        }

        /// <summary>
        /// Конвертер значений. Используется для обработки входящих значений.
        /// </summary>
        public ConvertCollectionValue? Converter { get; set; }
        /// <summary>
        /// Список объектов
        /// </summary>
        public IEnumerable Enumerable { get; }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int Count => (int)_CountProperty.GetValue(Enumerable);
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
        /// <param name="Index"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public object this[int Index]
        {
            get
            {
                _MethodParams[0] = Index;
                return _ItemProperty.GetValue(Enumerable, _MethodParams);
            }
            set
            {
                _MethodParams[0] = Index;
                _ItemProperty.SetValue(Enumerable, Convert(value, Index), _MethodParams);
            }
        }

        private readonly MethodInfo _RemoveAtMethod;
        private readonly MethodInfo _RemoveMethod;
        private readonly MethodInfo _InsertMethod;
        private readonly MethodInfo _AddMethod;
        private readonly MethodInfo _ClearMethod;
        private readonly PropertyInfo _CountProperty;
        private readonly PropertyInfo _ItemProperty;
        private readonly object?[] _MethodParams = new object?[1];
        private readonly object?[] _MethodParams2 = new object?[2];

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Contains(object? Value)
        {
            return IndexOf(Value) != -1;
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int IndexOf(object? Value)
        {
            for (int i = 0; i < Count; i++)
            {
                if (Value?.Equals(this[i]) == true)
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public int Add(object? Item)
        {
            int startCount = Count;

            _MethodParams[0] = Convert(Item, startCount);
            _AddMethod.Invoke(Enumerable, _MethodParams);

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
        /// <param name="Index"><inheritdoc/></param>
        /// <param name="Item"><inheritdoc/></param>
        public void Insert(int Index, object? Item)
        {
            _MethodParams2[0] = Index;
            _MethodParams2[1] = Convert(Item, Index);
            _InsertMethod.Invoke(Enumerable, _MethodParams2);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Clear()
        {
            _ClearMethod.Invoke(Enumerable, Array.Empty<object>());
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Index"><inheritdoc/></param>
        public void RemoveAt(int Index)
        {
            _MethodParams[0] = Index;
            _RemoveAtMethod.Invoke(Enumerable, _MethodParams);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Value"><inheritdoc/></param>
        public void Remove(object? Value)
        {
            _MethodParams[0] = Value;
            _RemoveMethod.Invoke(Enumerable, _MethodParams);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Array"><inheritdoc/></param>
        /// <param name="Index"><inheritdoc/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void CopyTo(Array Array, int Index)
        {
            if (Array == null)
            {
                throw new ArgumentNullException(nameof(Array));
            }

            for (int i = 0; i < Count; i++)
            {
                Array.SetValue(this[i], i + Index);
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Очистить экземпляр объекта
        /// </summary>
        /// <param name="isDisposing">true - ручной вызов, false - вызов сборщиком мусора</param>
        protected virtual void Dispose(bool isDisposing)
        {
            _MethodParams[0] = null;
            _MethodParams2[0] = null;
            _MethodParams2[1] = null;
        }

        private object? Convert(object? Value, int Index)
        {
            if (Converter != null)
            {
                Value = Converter(Value, Index, this);
            }

            return Value;
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
