using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Acly
{
    /// <summary>
    /// Синхронизатор списков
    /// </summary>
    public class CollectionSynchronizer<T1, T2> : Disposable, IEnumerable<T1>, INotifyPropertyChanged
    {
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="first"><inheritdoc cref="_firstCollection"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged first)
            : this(first, new ObservableCollection<T1>())
        {
        }
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="first"><inheritdoc cref="_firstCollection"/></param>
        /// <param name="converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged first, IValueConverter<T1, T2> converter)
            : this(first, new ObservableCollection<T2>(), converter)
        {
        }
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="first"><inheritdoc cref="_firstCollection"/></param>
        /// <param name="converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged first, ICollectionValueConverter<T1, T2> converter)
            : this(first, new ObservableCollection<T2>(), converter)
        {
        }
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="first"><inheritdoc cref="_firstCollection"/></param>
        /// <param name="second"><inheritdoc cref="_secondCollection"/></param>
        /// <param name="converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged first, INotifyCollectionChanged second, IValueConverter<T1, T2>? converter)
            : this(first, second, (ICollectionValueConverter<T1, T2>?)CreateIfNotNull(converter))
        {
        }
        /// <summary>
        /// Создать новый экземпляр синхронизатора коллекций
        /// </summary>
        /// <param name="first"><inheritdoc cref="_firstCollection"/></param>
        /// <param name="second"><inheritdoc cref="_secondCollection"/></param>
        /// <param name="converter"><inheritdoc cref="Converter"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CollectionSynchronizer(INotifyCollectionChanged first, INotifyCollectionChanged second, ICollectionValueConverter<T1, T2>? converter = null)
        {
            bool typesNotEquals = typeof(T1) != typeof(T2);

            if (typesNotEquals && converter == null)
            {
                throw new ArgumentNullException("При указании разных типов необходимо указать и конвертер!", nameof(converter));
            }

            FirstCollection = first ?? throw new ArgumentNullException(nameof(first));
            SecondCollection = second ?? throw new ArgumentNullException(nameof(second));
            Converter = converter;

            if (first is not IEnumerable collection)
            {
                throw new ArgumentException($"Объект не является списком!", nameof(first));
            }
            if (second is not IEnumerable collection2)
            {
                throw new ArgumentException($"Объект не является списком!", nameof(second));
            }

            FirstCollection.CollectionChanged += FirstCollectionChanged;
            SecondCollection.CollectionChanged += SecondCollectionChanged;

            ConvertCollectionValue? convertMethod = null;
            ConvertCollectionValue? convertBackMethod = null;
            _firstCollection = new(collection);
            _secondCollection = new(collection2);

            if (typesNotEquals && converter != null)
            {
                convertMethod = (obj, index, list) =>
                {
                    if (obj != null)
                    {
                        return converter.Convert((T1)obj, index, _firstCollection, _secondCollection);
                    }

                    return null;
                };
                convertBackMethod = (obj, index, list) =>
                {
                    if (obj != null)
                    {
                        return converter.ConvertBack((T2)obj, index, _firstCollection, _secondCollection);
                    }

                    return null;
                };
            }

            _firstCollection.Converter = convertBackMethod;
            _secondCollection.Converter = convertMethod;

            SyncFirstToSecond();
        }

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
            get => field;
            private set
            {
                if (field != value)
                {
                    OnPropertyChanging(nameof(IsUpdating));
                    field = value;
                    OnPropertyChanged(nameof(IsUpdating));
                }
            }
        }

        private readonly ReflectionList _firstCollection;
        private readonly ReflectionList _secondCollection;

        #region Управление

        /// <summary>
        /// Синхронизировать значения второй коллекции с первой.
        /// </summary>
        public void SyncFirstToSecond()
        {
            ForceSync(_firstCollection, _secondCollection);
        }
        /// <summary>
        /// Синхронизировать значения первой коллекции со второй.
        /// </summary>
        public void SyncSecondToFirst()
        {
            ForceSync(_secondCollection, _firstCollection);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="isDisposing"><inheritdoc/></param>
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);

            FirstCollection.CollectionChanged -= FirstCollectionChanged;
            SecondCollection.CollectionChanged -= SecondCollectionChanged;

            _firstCollection.Dispose();
            _secondCollection.Dispose();
        }

        private bool TrySync(ReflectionList from, ReflectionList to, NotifyCollectionChangedEventArgs args)
        {
            if (IsUpdating)
            {
                return false;
            }

            try
            {
                IsUpdating = true;
                Sync(from, to, args);
            }
            catch (Exception error)
            {
                LogError(error);
            }
            finally
            {
                IsUpdating = false;
            }

            return true;
        }
        private void ForceSync(ReflectionList from, ReflectionList to)
        {
            IsUpdating = true;

            try
            {
                to.Clear();

                foreach (var Item in from)
                {
                    to.Add(Item);
                }
            }
            catch (Exception error)
            {
                LogError(error);
            }
            finally
            {
                IsUpdating = false;
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

        private void FirstCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TrySync(_firstCollection, _secondCollection, e);
        }
        private void SecondCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TrySync(_secondCollection, _firstCollection, e);
        }

        #endregion

        #region Статика

        private static void Sync(ReflectionList from, ReflectionList to, NotifyCollectionChangedEventArgs args)
        {
            switch (args.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (args.NewItems != null)
                    {
                        for (int i = 0; i < args.NewItems.Count; i++)
                        {
                            var newIndex = args.NewStartingIndex + i;
                            if (newIndex != -1 && newIndex < to.Count)
                            {
                                to.Insert(newIndex, args.NewItems[i]);
                            }
                            else
                            {
                                to.Add(args.NewItems[i]);
                            }
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (args.OldStartingIndex >= 0 && args.OldStartingIndex < to.Count)
                    {
                        for (int i = 0; i < args.OldItems.Count; i++)
                        {
                            to.RemoveAt(args.OldStartingIndex);
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    if (args.NewStartingIndex >= 0 && args.NewStartingIndex < to.Count)
                    {
                        to[args.NewStartingIndex] = args.NewItems[0];
                    }
                    break;

                case NotifyCollectionChangedAction.Move:
                    if (args.OldStartingIndex >= 0 && args.OldStartingIndex < to.Count &&
                        args.NewStartingIndex >= 0 && args.NewStartingIndex <= to.Count)
                    {
                        var item = to[args.OldStartingIndex];
                        to.RemoveAt(args.OldStartingIndex);
                        to.Insert(args.NewStartingIndex, item);
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    to.Clear();
                    foreach (var item in from)
                    {
                        to.Add(item);
                    }
                    break;
            }
        }

        #endregion

        #region Статика

        private static CollectionValueConverterToCommonConverter<T1, T2>? CreateIfNotNull(IValueConverter<T1, T2>? converter)
        {
            if (converter == null)
            {
                return null;
            }

            return new CollectionValueConverterToCommonConverter<T1, T2>(converter);
        }

        #endregion
    }
}
