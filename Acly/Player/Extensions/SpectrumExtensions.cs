using Acly.Player.Spectrum;
using System;

namespace Acly.Player
{
	/// <summary>
	/// Класс с методами расширения для работы с данными спектра
	/// </summary>
	public static class SpectrumExtensions
	{
		/// <summary>
		/// Получить данные спектра указанного размера с применением указанного FFT окна
		/// </summary>
		/// <param name="Player">Плеер из которого берутся данные спектра</param>
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Данные спектра</returns>
		/// <returns>Готовые к использованию данные спектра</returns>
		/// <exception cref="ArgumentNullException">Ссылка на плеер не указывает на его экземпляр</exception>
		public static float[] GetFilteredSpectrumData(this ISimplePlayer Player, int Size, SpectrumWindow Window = SpectrumWindow.Rectangular)
		{
			if (Player == null)
			{
				throw new ArgumentNullException(nameof(Player) + " не указан");
			}

			float[] Data = Player.GetSpectrumData(Size * Size, Window);
			float[] Result = new float[Size];

			for (int i = 0; i < Size; i++)
			{
				Result[i] = Data[i * i];
			}

			return Result;
		}
		/// <summary>
		/// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
		/// </summary>
		/// <param name="Player">Плеер из которого берутся данные спектра</param>
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="SmoothAmount">Степень сглаживания. 
		/// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
		/// </param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Готовые к использованию данные спектра</returns>
		/// <exception cref="ArgumentNullException">Ссылка на плеер не указывает на его экземпляр</exception>
		/// <exception cref="ArgumentException">Степень сглаживания меньше нуля</exception>
		public static float[] GetFilteredSpectrumData(this ISimplePlayer Player, int Size, int SmoothAmount, SpectrumWindow Window = SpectrumWindow.Rectangular)
		{
			if (Player == null)
			{
				throw new ArgumentNullException(nameof(Player) + " не указан");
			}
			if (SmoothAmount < 0)
			{
				throw new ArgumentException("Сглаживание не может быть отрицательным");
			}

			float[] Data = Player.GetFilteredSpectrumData(Size, Window);
			Data = ArrayWork.Smooth(Data, SmoothAmount);

			float Average = ArrayWork.Average(Data) * 2;
			Data = ArrayWork.Multiply(Data, Average);

			return Data;
		}
	}
}
