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
		/// <param name="From">Начальное значение Обычно это 0</param>
		/// <param name="To">Конечное значение. Обычно - 0 или 100</param>
		/// <param name="CompletedPercent">Процент выполненной работы от 0 до 1</param>
		/// <returns>Прогресс выполненной работы между <paramref name="From"/> и <paramref name="To"/></returns>
		public static float Range(float From, float To, float CompletedPercent)
		{
			return Helper.Lerp(From, To, CompletedPercent);
		}
		/// <summary>
		/// Получить процент выполненной работы
		/// </summary>
		/// <param name="TotalAmount">Общее количество действий</param>
		/// <param name="Completed">Количество уже выполненных действий</param>
		/// <returns>Процент выполненной работы от 0 до 1</returns>
		public static float FromAmount(int TotalAmount, int Completed)
		{
			return (float)Completed / TotalAmount;
		}
		/// <summary>
		/// Получить прогресс с учётом выполненного количества действий и прогресса текущего действия
		/// </summary>
		/// <param name="TotalAmount">Общее количество действий</param>
		/// <param name="Current">Текущее действие</param>
		/// <param name="Progress">Прогресс текущего действия от 0 до 1</param>
		/// <returns>Процент выполненной работы от 0 до 1</returns>
		public static float FromAmountsRange(int TotalAmount, int Current, float Progress)
		{
			float OneJobPercent = 1f / TotalAmount;
			float CurrentPercent = OneJobPercent * Progress;

			return FromAmount(TotalAmount, Current) + CurrentPercent;
		}
	}
}
