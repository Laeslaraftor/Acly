using System;
using System.Collections;

namespace Acly
{
    /// <summary>
    /// Фильтр элементов коллекции основанный на делегате
    /// </summary>
    public class RelayCollectionItemsFilter : ICollectionItemsFilter
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public event EventHandler? FilterChanged;

        /// <summary>
        /// Фильтр элементов
        /// </summary>
        public Func<IEnumerable, object, bool>? Filter
        {
            get => _Filter;
            set
            {
                if (_Filter != value)
                {
                    _Filter = value;
                }
            }
        }

        private Func<IEnumerable, object, bool>? _Filter;

        #region Управление

        /// <summary>
        /// Обновить фильтр
        /// </summary>
        /// <param name="Args">Аргументы события изменения фильтра</param>
        public void Update(EventArgs Args)
        {
            FilterChanged?.Invoke(this, Args);
        }
        /// <summary>
        /// Обновить фильтр
        /// </summary>
        public void Update() => Update(EventArgs.Empty);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Collection"><inheritdoc/></param>
        /// <param name="Item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Check(IEnumerable Collection, object Item)
        {
            var Filter = this.Filter;

            if (Filter == null)
            {
                return true;
            }

            return Filter(Collection, Item);
        }

        #endregion
    }
}
