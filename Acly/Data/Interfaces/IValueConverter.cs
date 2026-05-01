namespace Acly
{
    /// <summary>
    /// Интерфейс конвертера значений
    /// </summary>
    /// <typeparam name="T1">Тип значения 1</typeparam>
    /// <typeparam name="T2">Тип значения 2</typeparam>
    public interface IValueConverter<T1, T2>
    {
        /// <summary>
        /// Конвертировать значение типа 1 в значение типа 2
        /// </summary>
        /// <param name="value">Значение типа 1</param>
        /// <returns>Конвертированное значение типа 2</returns>
        public T2 Convert(T1 value);
        /// <summary>
        /// Конвертировать значение типа 2 в значение типа 1
        /// </summary>
        /// <param name="value">Значение типа 2</param>
        /// <returns>Конвертированное значение типа 1</returns>
        public T1 ConvertBack(T2 value);
    }
}
