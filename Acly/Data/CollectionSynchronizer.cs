using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Acly
{
    /// <summary>
    /// Синхронизатор списков
    /// </summary>
    public class CollectionSynchronizer<T1, T2> : IEnumerable<T1>, INotifyPropertyChanged, IDisposable
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
        /// <param name="Converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged First, ICollectionValueConverter<T1, T2> Converter)
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
        public CollectionSynchronizer(INotifyCollectionChanged First, INotifyCollectionChanged Second, IValueConverter<T1, T2>? Converter)
            : this(First, Second, CreateIfNotNull(Converter))
        {
        }
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="First"><inheritdoc cref="_FirstCollection"/></param>
        /// <param name="Second"><inheritdoc cref="_SecondCollection"/></param>
        /// <param name="Converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged First, INotifyCollectionChanged Second, ICollectionValueConverter<T1, T2>? Converter = null)
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

            ConvertCollectionValue? ConvertMethod = null;
            ConvertCollectionValue? ConvertBackMethod = null;
            _FirstCollection = new(Collection);
            _SecondCollection = new(Collection2);

            if (TypesNotEquals && Converter != null)
            {
                ConvertMethod = (Obj, Index, List) =>
                {
                    if (Obj != null)
                    {
                        return Converter.Convert((T1)Obj, Index, _FirstCollection, _SecondCollection);
                    }

                    return null;
                };
                ConvertBackMethod = (Obj, Index, List) =>
                {
                    if (Obj != null)
                    {
                        return Converter.ConvertBack((T2)Obj, Index, _FirstCollection, _SecondCollection);
                    }

                    return null;
                };
            }

            _FirstCollection.Converter = ConvertBackMethod;
            _SecondCollection.Converter = ConvertMethod;

            SyncFirstToSecond();
        }
        /// <summary>
        /// Очистка синхронизатора
        /// </summary>
        ~CollectionSynchronizer()
        {
            Dispose(false);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [field: NonSerialized] public event PropertyChangedEventHandler? PropertyChanged;

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
        public ICollectionValueConverter<T1, T2>? Converter { get; }
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
        /// Синхронизировать значения второй коллекции с первой.
        /// </summary>
        public void SyncFirstToSecond()
        {
            ForceSync(_FirstCollection, _SecondCollection);
        }
        /// <summary>
        /// Синхронизировать значения первой коллекции со второй.
        /// </summary>
        public void SyncSecondToFirst()
        {
            ForceSync(_SecondCollection, _FirstCollection);
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
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
            finally
            {
                IsUpdating = false;
            }

            return true;
        }
        private void ForceSync(ReflectionList From, ReflectionList To)
        {
            _IsUpdating = true;

            try
            {
                To.Clear();

                foreach (var Item in From)
                {
                    To.Add(Item);
                }
            }
            catch (Exception Error)
            {
                Debug.WriteLine(Error);
            }
            finally
            {
                _IsUpdating = false;
            }
        }

        #endregion

        #region Перечисление

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public IEnumerator<T1> GetEnumerator()
        {
            return ((IEnumerable<T1>)FirstCollection).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

        #region Статика

        private static ICollectionValueConverter<T1, T2>? CreateIfNotNull(IValueConverter<T1, T2>? Converter)
        {
            if (Converter == null)
            {
                return null;
            }

            return new CollectionValueConverterToCommonConverter<T1, T2>(Converter);
        }

        #endregion
    }
}
