using System.Collections;

namespace Acly
{
    /// <summary>
    /// Конвертер значений списка, основанный на общем конвертере значений
    /// </summary>
    /// <typeparam name="T1">Тип значения 1</typeparam>
    /// <typeparam name="T2">Тип значения 2</typeparam>
    /// <remarks>
    /// Создать новый экземпляр конвертера значений списка
    /// </remarks>
    /// <param name="converter">Общий конвертер значений</param>
    public class CollectionValueConverterToCommonConverter<T1, T2>(IValueConverter<T1, T2> converter)
        : IValueConverter<T1, T2>, ICollectionValueConverter<T1, T2>
    {

        /// <summary>
        /// Общий конвертер значений
        /// </summary>
        public IValueConverter<T1, T2> Converter { get; } = converter;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T2 Convert(T1 value)
        {
            return Converter.Convert(value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"><inheritdoc/></param>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="firstCollection"><inheritdoc/></param>
        /// <param name="secondCollection"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T2 Convert(T1 value, int index, IList firstCollection, IList secondCollection)
        {
            return Convert(value);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T1 ConvertBack(T2 value)
        {
            return Converter.ConvertBack(value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"><inheritdoc/></param>
        /// <param name="index"><inheritdoc/></param>
        /// <param name="firstCollection"><inheritdoc/></param>
        /// <param name="secondCollection"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T1 ConvertBack(T2 value, int index, IList firstCollection, IList secondCollection)
        {
            return ConvertBack(value);
        }
    }
}
