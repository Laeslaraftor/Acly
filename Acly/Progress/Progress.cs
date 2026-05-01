namespace Acly
{
    /// <summary>
    /// Класс для работы с прогрессом
    /// </summary>
    public static class Progress
    {
        /// <summary>
        /// Выполнение значение при указанном выполненном проценте
        /// </summary>
        /// <param name="from">Начальное значение Обычно это 0</param>
        /// <param name="to">Конечное значение. Обычно - 0 или 100</param>
        /// <param name="completedPercent">Процент выполненной работы от 0 до 1</param>
        /// <returns>Прогресс выполненной работы между <paramref name="from"/> и <paramref name="to"/></returns>
        public static float Range(float from, float to, float completedPercent)
        {
            return Helper.Lerp(from, to, completedPercent);
        }
        /// <summary>
        /// Получить процент выполненной работы
        /// </summary>
        /// <param name="totalAmount">Общее количество действий</param>
        /// <param name="completed">Количество уже выполненных действий</param>
        /// <returns>Процент выполненной работы от 0 до 1</returns>
        public static float FromAmount(int totalAmount, int completed)
        {
            return (float)completed / totalAmount;
        }
        /// <summary>
        /// Получить прогресс с учётом выполненного количества действий и прогресса текущего действия
        /// </summary>
        /// <param name="totalAmount">Общее количество действий</param>
        /// <param name="current">Текущее действие</param>
        /// <param name="progress">Прогресс текущего действия от 0 до 1</param>
        /// <returns>Процент выполненной работы от 0 до 1</returns>
        public static float FromAmountsRange(int totalAmount, int current, float progress)
        {
            float OneJobPercent = 1f / totalAmount;
            float CurrentPercent = OneJobPercent * progress;

            return FromAmount(totalAmount, current) + CurrentPercent;
        }
    }
}
