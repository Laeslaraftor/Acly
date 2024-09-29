using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Acly.Player.Spectrum
{
	/// <summary>
	/// Класс с методами для работы с массивами
	/// </summary>
	public static class ArrayWork
	{
		/// <summary>
		/// Сгладить массив
		/// </summary>
		/// <param name="Array">Массив для сглаживания</param>
		/// <param name="Size">Степень сглаживания. 
		/// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
		/// </param>
		/// <returns>Сглаженный массив</returns>
		public static float[] Smooth(float[] Array, int Size)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(nameof(Array), "Массив не указан");
			}

			float[] Result = new float[Array.Length];

			for (int i = 0; i < Array.Length; i++)
			{
				Result[i] = RangeAverage(Array, i, Size);
			}

			return Result;
		}

		/// <summary>
		/// Получить среднее арифметическое значение отрезка массива
		/// </summary>
		/// <param name="Array">Массив со значениями</param>
		/// <param name="Index">Начало отрезка</param>
		/// <param name="Size">Длина отрезка</param>
		/// <returns>Среднее арифметическое значение отрезка массива</returns>
		public static float RangeAverage(float[] Array, int Index, int Size)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(nameof(Array), "Массив не указан");
			}

			float Result = Array[Index];
			int Amount = 1;

			Collection<float> Left = RangeValues(Array, Index - Size, Size);
			Collection<float> Right = RangeValues(Array, Index, Size);

			Amount += Left.Count + Right.Count;
			Result += RangeSum(Left) + RangeSum(Right);

			return Result / Amount;
		}
		/// <summary>
		/// Получить значения массива на определённом отрезке
		/// </summary>
		/// <typeparam name="T">Тип данных массива</typeparam>
		/// <param name="Array">Массив со значениями</param>
		/// <param name="From">Начало отрезка</param>
		/// <param name="Amount">Длина отрезка</param>
		/// <returns>Значения массива на определённом отрезке</returns>
		public static Collection<T> RangeValues<T>(T[] Array, int From, int Amount)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(nameof(Array), "Массив не указан");
			}

			Collection<T> Result = new();

			for (int i = From; i < From + Amount; i++)
			{
				if (i < 0 || i >= Array.Length)
				{
					continue;
				}

				Result.Add(Array[i]);
			}

			return Result;
		}
		/// <summary>
		/// Получить сумму чисел в перечислении
		/// </summary>
		/// <param name="Array">Перечисление чисел</param>
		/// <returns>Сумма чисел в перечислении</returns>
		public static float RangeSum(IEnumerable<float> Array)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(nameof(Array), "Перечисление не указано");
			}

			float Result = 0;

			foreach (var Val in Array)
			{
				Result += Val;
			}

			return Result;
		}

		/// <summary>
		/// Получить среднее арифметическое массива чисел
		/// </summary>
		/// <param name="Array">Массив чисел</param>
		/// <returns>Среднее арифметическое массива чисел</returns>
		public static float Average(float[] Array)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(nameof(Array), "Массив не указан");
			}

			float Average = 0;

			foreach (var Val in Array)
			{
				Average += Val;
			}

			return Average / Array.Length;
		}
		/// <summary>
		/// Умножить все числа в массиве на указанное значение
		/// </summary>
		/// <param name="Array">Массив чисел</param>
		/// <param name="Value">Множитель</param>
		/// <returns>Массив с умноженными значениями</returns>
		public static float[] Multiply(float[] Array, float Value)
		{
			if (Array == null)
			{
				throw new ArgumentNullException(nameof(Array), "Массив не указан");
			}

			for (int i = 0; i < Array.Length; i++)
			{
				Array[i] *= Value;
			}

			return Array;
		}
	}
}
