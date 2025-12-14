using System.Collections;

namespace Acly
{
    /// <summary>
    /// Конвертер значений списка, основанный на общем конвертере значений
    /// </summary>
    /// /// <typeparam name="T1">Тип значения 1</typeparam>
    /// <typeparam name="T2">Тип значения 2</typeparam>
    public class CollectionValueConverterToCommonConverter<T1, T2> : IValueConverter<T1, T2>, ICollectionValueConverter<T1, T2>
    {
        /// <summary>
        /// Создать новый экземпляр конвертера значений списка
        /// </summary>
        /// <param name="Converter">Общий конвертер значений</param>
        public CollectionValueConverterToCommonConverter(IValueConverter<T1, T2> Converter)
        {
            this.Converter = Converter;
        }

        /// <summary>
        /// Общий конвертер значений
        /// </summary>
        public IValueConverter<T1, T2> Converter { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T2 Convert(T1 Value)
        {
            return Converter.Convert(Value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Value"><inheritdoc/></param>
        /// <param name="Index"><inheritdoc/></param>
        /// <param name="FirstCollection"><inheritdoc/></param>
        /// <param name="SecondCollection"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T2 Convert(T1 Value, int Index, IList FirstCollection, IList SecondCollection)
        {
            return Convert(Value);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Value"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T1 ConvertBack(T2 Value)
        {
            return Converter.ConvertBack(Value);
        }
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Value"><inheritdoc/></param>
        /// <param name="Index"><inheritdoc/></param>
        /// <param name="FirstCollection"><inheritdoc/></param>
        /// <param name="SecondCollection"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public T1 ConvertBack(T2 Value, int Index, IList FirstCollection, IList SecondCollection)
        {
            return ConvertBack(Value);
        }
    }
}
