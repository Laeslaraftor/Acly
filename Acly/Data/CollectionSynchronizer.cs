using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reflection;

namespace Acly
{
    /// <summary>
    /// Синхронизатор списков
    /// </summary>
    public class CollectionSynchronizer<T1, T2> : INotifyPropertyChanged, IDisposable
    {
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="First"><inheritdoc cref="_FirstCollection"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged First) 
            : this(First, new ObservableCollection<T1>())
        {
        }
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="First"><inheritdoc cref="_FirstCollection"/></param>
        /// <param name="Converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged First, IValueConverter<T1, T2> Converter) 
            : this(First, new ObservableCollection<T2>(), Converter)
        {
        }
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="First"><inheritdoc cref="_FirstCollection"/></param>
        /// <param name="Second"><inheritdoc cref="_SecondCollection"/></param>
        /// <param name="Converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged First, INotifyCollectionChanged Second, IValueConverter<T1, T2>? Converter = null)
        {
            bool TypesNotEquals = typeof(T1) != typeof(T2);

            if (TypesNotEquals && Converter == null)
            {
                throw new ArgumentNullException("При указании разных типов необходимо указать и конвертер!", nameof(Converter));
            }

            FirstCollection = First ?? throw new ArgumentNullException(nameof(First));
            SecondCollection = Second ?? throw new ArgumentNullException(nameof(Second));
            this.Converter = Converter;

            if (First is not IEnumerable Collection)
            {
                throw new ArgumentException($"Объект не является списком!", nameof(First));
            }
            if (Second is not IEnumerable Collection2)
            {
                throw new ArgumentException($"Объект не является списком!", nameof(Second));
            }

            FirstCollection.CollectionChanged += FirstCollectionChanged;
            SecondCollection.CollectionChanged += SecondCollectionChanged;

            Func<object?, object?> ConvertMethod;
            Func<object?, object?> ConvertBackMethod;

            if (TypesNotEquals && Converter != null)
            {
                ConvertMethod = o =>
                {
                    if (o != null)
                    {
                        return Converter.Convert((T1)o);
                    }

                    return null;
                };
                ConvertBackMethod = o =>
                {
                    if (o != null)
                    {
                        return Converter.ConvertBack((T2)o);
                    }

                    return null;
                };
            }
            else
            {
                ConvertMethod = o => o;
                ConvertBackMethod = ConvertMethod;
            }

            _FirstCollection = new(Collection, ConvertBackMethod);
            _SecondCollection = new(Collection2, ConvertMethod);

            SynchronizeFromFirstToSecond();
        }
        /// <summary>
        /// Очистка синхронизатора
        /// </summary>
#pragma warning disable CA1063 // Правильно реализуйте IDisposable
        ~CollectionSynchronizer()
#pragma warning restore CA1063 // Правильно реализуйте IDisposable
        {
            Dispose(false);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Первая отслеживаемая коллекция
        /// </summary>
        public INotifyCollectionChanged FirstCollection { get; }
        /// <summary>
        /// Вторая отслеживаемая коллекция
        /// </summary>
        public INotifyCollectionChanged SecondCollection { get; }
        /// <summary>
        /// Конвертер значений
        /// </summary>
        public IValueConverter<T1, T2>? Converter { get; }
        /// <summary>
        /// Обновляются ли сейчас коллекции
        /// </summary>
        public bool IsUpdating
        {
            get => _IsUpdating;
            private set
            {
                if (_IsUpdating != value)
                {
                    _IsUpdating = value;
                    InvokePropertyChanged(nameof(IsUpdating));
                }
            }
        }

        private readonly ReflectionList _FirstCollection;
        private readonly ReflectionList _SecondCollection;
        private bool _IsUpdating;

        #region Управление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Очистка синхронизатора
        /// </summary>
        /// <param name="IsDisposing"></param>
        protected virtual void Dispose(bool IsDisposing)
        {
            FirstCollection.CollectionChanged -= FirstCollectionChanged;
            SecondCollection.CollectionChanged -= SecondCollectionChanged;

            _FirstCollection.Dispose();
            _SecondCollection.Dispose();
        }

        private bool TrySync(ReflectionList From, ReflectionList To, NotifyCollectionChangedEventArgs Args)
        {
            if (IsUpdating)
            {
                return false;
            }

            try
            {
                IsUpdating = true;
                Sync(From, To, Args);
            }
            finally
            {
                IsUpdating = false;
            }

            return true;
        }
        private void SynchronizeFromFirstToSecond()
        {
            _IsUpdating = true;

            try
            {
                _SecondCollection.Clear();

                foreach (var Item in _FirstCollection)
                {
                    _SecondCollection.Add(Item);
                }
            }
            finally
            {
                _IsUpdating = false;
            }
        }

        #endregion

        #region События

        private void InvokePropertyChanged(string PropertyName)
        {
            PropertyChanged?.Invoke(this, new(PropertyName));
        }
        private void FirstCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TrySync(_FirstCollection, _SecondCollection, e);
        }
        private void SecondCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TrySync(_SecondCollection, _FirstCollection, e);
        }

        #endregion

        #region Статика

        private static void Sync(ReflectionList From, ReflectionList To, NotifyCollectionChangedEventArgs Args)
        {
            switch (Args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (Args.NewItems != null)
                    {
                        for (int i = 0; i < Args.NewItems.Count; i++)
                        {
                            var newIndex = Args.NewStartingIndex + i;
                            if (newIndex <= To.Count)
                            {
                                To.Insert(newIndex, Args.NewItems[i]);
                            }
                            else
                            {
                                To.Add(Args.NewItems[i]);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (Args.OldStartingIndex >= 0 && Args.OldStartingIndex < To.Count)
                    {
                        for (int i = 0; i < Args.OldItems.Count; i++)
                        {
                            To.RemoveAt(Args.OldStartingIndex);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (Args.NewStartingIndex >= 0 && Args.NewStartingIndex < To.Count)
                    {
                        To[Args.NewStartingIndex] = Args.NewItems[0];
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (Args.OldStartingIndex >= 0 && Args.OldStartingIndex < To.Count &&
                        Args.NewStartingIndex >= 0 && Args.NewStartingIndex <= To.Count)
                    {
                        var item = To[Args.OldStartingIndex];
                        To.RemoveAt(Args.OldStartingIndex);
                        To.Insert(Args.NewStartingIndex, item);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    To.Clear();
                    foreach (var item in From)
                    {
                        To.Add(item);
                    }
                    break;
            }
        }

        #endregion

        #region Классы

        private sealed class ReflectionList : IEnumerable, IDisposable
        {
            public ReflectionList(IEnumerable List, Func<object?, object?> Converter)
            {
                Enumerable = List;
                this.Converter = Converter;

                Type ListType = List.GetType();
                var properties = ListType.GetProperties();

                if (ListType.TryFindMethod("Add", out var AddMethod) &&
                    ListType.TryFindMethod("Insert", out var InsertMethod) &&
                    ListType.TryFindMethod("RemoveAt", out var RemoveAtMethod) &&
                    ListType.TryFindMethod("Clear", out var ClearMethod) &&
                    ListType.TryFindProperty("Count", out var CountProperty) &&
                    ListType.TryFindProperty("Item", out var ItemProperty))
                {
                    _AddMethod = AddMethod;
                    _InsertMethod = InsertMethod;
                    _RemoveAtMethod = RemoveAtMethod;
                    _ClearMethod = ClearMethod;
                    _CountProperty = CountProperty;
                    _ItemProperty = ItemProperty;
                }
                else
                {
                    throw new ArgumentException($"Объект не является списком!", nameof(List));
                }
            }
            ~ReflectionList()
            {
                Dispose();
            }

            public Func<object?, object?> Converter { get; }
            public IEnumerable Enumerable { get; }
            public int Count => (int)_CountProperty.GetValue(Enumerable);
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
                    _ItemProperty.SetValue(Enumerable, Converter(value), _MethodParams);
                }
            }

            private readonly MethodInfo _RemoveAtMethod;
            private readonly MethodInfo _InsertMethod;
            private readonly MethodInfo _AddMethod;
            private readonly MethodInfo _ClearMethod;
            private readonly PropertyInfo _CountProperty;
            private readonly PropertyInfo _ItemProperty;
            private readonly object?[] _MethodParams = new object?[1];
            private readonly object?[] _MethodParams2 = new object?[2];

            public void Add(object? Item)
            {
                _MethodParams[0] = Converter(Item);
                _AddMethod.Invoke(Enumerable, _MethodParams);
            }
            public void Clear()
            {
                _ClearMethod.Invoke(Enumerable, Array.Empty<object>());
            }
            public void RemoveAt(int Index)
            {
                _MethodParams[0] = Index;
                _RemoveAtMethod.Invoke(Enumerable, _MethodParams);
            }
            public void Insert(int Index, object? Item)
            {
                _MethodParams2[0] = Index;
                _MethodParams2[1] = Converter(Item);
                _InsertMethod.Invoke(Enumerable, _MethodParams2);
            }
            public IEnumerator GetEnumerator()
            {
                return Enumerable.GetEnumerator();
            }

            public void Dispose()
            {
                _MethodParams[0] = null;
                _MethodParams2[0] = null;
                _MethodParams2[1] = null;

                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
