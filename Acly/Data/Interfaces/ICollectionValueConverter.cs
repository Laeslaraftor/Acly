using System.Collections;

namespace Acly
{
    /// <summary>
    /// Интерфейс конвертера значений списков
    /// </summary>
    /// <typeparam name="T1">Тип значения 1</typeparam>
    /// <typeparam name="T2">Тип значения 2</typeparam>
    public interface ICollectionValueConverter<T1, T2>
    {
        /// <summary>
        /// Конвертировать значение типа 1 в значение типа 2
        /// </summary>
        /// <param name="value">Значение типа 1</param>
        /// <param name="index">Индекс значения в первом списке. По этому же индексу будет записано конвертированное значение во второй список</param>
        /// <param name="firstCollection">Список, содержащий конвертируемое значение</param>
        /// <param name="secondCollection">Список в который будет записано конвертированное значение</param>
        /// <returns>Конвертированное значение типа 2</returns>
        public T2 Convert(T1 value, int index, IList firstCollection, IList secondCollection);
        /// <summary>
        /// Конвертировать значение типа 2 в значение типа 1
        /// </summary>
        /// <param name="value">Значение типа 2</param>
        /// <param name="index">Индекс значения во втором списке. По этому же индексу будет записано конвертированное значение в первый список</param>
        /// <param name="firstCollection">Список в который будет записано конвертированное значение</param>
        /// <param name="secondCollection">Список, содержащий конвертируемое значение</param>
        /// <returns>Конвертированное значение типа 1</returns>
        public T1 ConvertBack(T2 value, int index, IList firstCollection, IList secondCollection);
    }
}
