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
            get => field;
            set
            {
                if (field != value)
                {
                    field = value;
                    Update();
                }
            }
        }

        #region Управление

        /// <summary>
        /// Обновить фильтр
        /// </summary>
        /// <param name="args">Аргументы события изменения фильтра</param>
        public void Update(EventArgs args)
        {
            FilterChanged?.Invoke(this, args);
        }
        /// <summary>
        /// Обновить фильтр
        /// </summary>
        public void Update() => Update(EventArgs.Empty);

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="collection"><inheritdoc/></param>
        /// <param name="item"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public bool Check(IEnumerable collection, object item)
        {
            var filter = Filter;

            if (filter == null)
            {
                return true;
            }

            return filter(collection, item);
        }

        #endregion
    }
}
