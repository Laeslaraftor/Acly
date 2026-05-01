using System;
using System.Collections.Generic;

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
        /// <param name="array">Массив для сглаживания</param>
        /// <param name="size">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <returns>Сглаженный массив</returns>
        public static float[] Smooth(float[] array, int size)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не указан");
            }

            float[] result = new float[array.Length];
            Smooth(array, result, size);

            return result;
        }
        /// <summary>
        /// Сгладить массив
        /// </summary>
        /// <param name="array">Массив для сглаживания</param>
        /// <param name="buffer">Массив в который будут записаны сглаженные значения</param>
        /// <param name="size">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <returns>Сглаженный массив</returns>
        public static void Smooth(float[] array, float[] buffer, int size)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не указан");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer), "Массив не указан");
            }
            if (array.Length != buffer.Length)
            {
                throw new ArgumentException("Длина массива и буфера сглаженных чисел должна быть одинаковой!");
            }

            Span<float> leftBuffer = stackalloc float[size];
            Span<float> rightBuffer = stackalloc float[size];

            for (int i = 0; i < array.Length; i++)
            {
                buffer[i] = RangeAverage(array, i, ref leftBuffer, ref rightBuffer);
            }
        }

        /// <summary>
        /// Получить среднее арифметическое значение отрезка массива
        /// </summary>
        /// <param name="array">Массив со значениями</param>
        /// <param name="index">Начало отрезка</param>
        /// <param name="leftBuffer">Буфер левых значений для подсчёта среднего значения</param>
        /// <param name="rightBuffer">Буфер правых значений для подсчёта среднего значения</param>
        /// <returns>Среднее арифметическое значение отрезка массива</returns>
        public static float RangeAverage(float[] array, int index, ref Span<float> leftBuffer, ref Span<float> rightBuffer)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не указан");
            }

            float result = array[index];
            int amount = 1;

            RangeValues(array, ref leftBuffer, index - leftBuffer.Length);
            RangeValues(array, ref rightBuffer, index);

            amount += leftBuffer.Length + rightBuffer.Length;
            result += RangeSum(leftBuffer) + RangeSum(rightBuffer);

            return result / amount;
        }
        /// <summary>
        /// Получить значения массива на определённом отрезке
        /// </summary>
        /// <typeparam name="T">Тип данных массива</typeparam>
        /// <param name="array">Массив со значениями</param>
        /// <param name="buffer">Буфер в который будут записаны значения массива на определённом отрезке</param>
        /// <param name="from">Начало отрезка</param>
        /// <returns>Значения массива на определённом отрезке</returns>
        public static void RangeValues<T>(T[] array, ref Span<T> buffer, int from)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не указан");
            }

            int length = buffer.Length;

            if (0 > from)
            {
                length += from;
                from = 0;
            }
            else
            {
                int totalLength = from + buffer.Length + 1;

                if (totalLength > array.Length)
                {
                    length -= totalLength - array.Length;
                }
            }
            if (0 >= length)
            {
                return;
            }

            for (int i = 0; i < length; i++)
            {
                buffer[i] = array[i + from];
            }
        }
        /// <summary>
        /// Получить сумму чисел в перечислении
        /// </summary>
        /// <param name="array">Перечисление чисел</param>
        /// <returns>Сумма чисел в перечислении</returns>
        public static float RangeSum(IEnumerable<float> array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Перечисление не указано");
            }

            float result = 0;

            foreach (var value in array)
            {
                result += value;
            }

            return result;
        }
        /// <summary>
        /// Получить сумму чисел в перечислении
        /// </summary>
        /// <param name="array">Перечисление чисел</param>
        /// <returns>Сумма чисел в перечислении</returns>
        public static float RangeSum(Span<float> array)
        {
            if (array.Length == 0)
            {
                return 0;
            }

            float result = 0;

            foreach (var value in array)
            {
                result += value;
            }

            return result;
        }

        /// <summary>
        /// Получить среднее арифметическое массива чисел
        /// </summary>
        /// <param name="array">Массив чисел</param>
        /// <returns>Среднее арифметическое массива чисел</returns>
        public static float Average(float[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не указан");
            }

            float average = 0;

            foreach (var value in array)
            {
                average += value;
            }

            return average / array.Length;
        }
        /// <summary>
        /// Умножить все числа в массиве на указанное значение
        /// </summary>
        /// <param name="array">Массив чисел</param>
        /// <param name="value">Множитель</param>
        /// <returns>Массив с умноженными значениями</returns>
        public static float[] Multiply(float[] array, float value)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array), "Массив не указан");
            }

            for (int i = 0; i < array.Length; i++)
            {
                array[i] *= value;
            }

            return array;
        }
    }
}
