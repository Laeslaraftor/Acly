using Acly.Player.Spectrum;
using System;

namespace Acly.Player
{
    /// <summary>
    /// Класс с методами расширения для работы с данными спектра
    /// </summary>
    public static class SpectrumExtensions
    {
        private static float[]? _lastSpectrumBuffer;
        private static float[]? _lastSpectrumSmoothBuffer;

        /// <summary>
        /// Получить данные спектра указанного размера с применением указанного FFT окна
        /// </summary>
        /// <param name="player">Плеер из которого берутся данные спектра</param>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        /// <returns>Готовые к использованию данные спектра</returns>
        /// <exception cref="ArgumentNullException">Ссылка на плеер не указывает на его экземпляр</exception>
        public static float[] GetFilteredSpectrumData(this ISimplePlayer player, int size, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            var buffer = _lastSpectrumBuffer;

            if (buffer == null || buffer.Length != size)
            {
                buffer = new float[size];
                _lastSpectrumBuffer = buffer;
            }

            player.GetFilteredSpectrumData(buffer, window);

            return buffer;
        }
        /// <summary>
        /// Получить данные спектра указанного размера с применением указанного FFT окна
        /// </summary>
        /// <param name="player">Плеер из которого берутся данные спектра</param>
        /// <param name="buffer">Буфер в который будут записаны данные спектра</param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        /// <returns>Готовые к использованию данные спектра</returns>
        /// <exception cref="ArgumentNullException">Ссылка на плеер не указывает на его экземпляр</exception>
        public static void GetFilteredSpectrumData(this ISimplePlayer player, float[] buffer, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player), nameof(player) + " не указан");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            float[] data = player.GetSpectrumData(buffer.Length * buffer.Length, window);

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = data[i * i];
            }
        }
        /// <summary>
        /// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
        /// </summary>
        /// <param name="player">Плеер из которого берутся данные спектра</param>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="smoothAmount">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <param name="window">FFT окно</param>
        /// <returns>Готовые к использованию данные спектра</returns>
        /// <exception cref="ArgumentNullException">Ссылка на плеер не указывает на его экземпляр</exception>
        /// <exception cref="ArgumentException">Степень сглаживания меньше нуля</exception>
        public static float[] GetFilteredSpectrumData(this ISimplePlayer player, int size, int smoothAmount, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            var buffer = _lastSpectrumBuffer;

            if (buffer == null || buffer.Length != size)
            {
                buffer = new float[size];
                _lastSpectrumBuffer = buffer;
            }

            player.GetFilteredSpectrumData(buffer, smoothAmount, window);

            return buffer;
        }
        /// <summary>
        /// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
        /// </summary>
        /// <param name="player">Плеер из которого берутся данные спектра</param>
        /// <param name="buffer">Буфер в который будут записаны итоговые данные спектра</param>
        /// <param name="smoothAmount">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <param name="window">FFT окно</param>
        /// <returns>Готовые к использованию данные спектра</returns>
        /// <exception cref="ArgumentNullException">Ссылка на плеер не указывает на его экземпляр</exception>
        /// <exception cref="ArgumentException">Степень сглаживания меньше нуля</exception>
        public static void GetFilteredSpectrumData(this ISimplePlayer player, float[] buffer, int smoothAmount, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            var tempBuffer = _lastSpectrumSmoothBuffer;

            if (tempBuffer == null || tempBuffer.Length != buffer.Length)
            {
                tempBuffer = new float[buffer.Length];
                _lastSpectrumSmoothBuffer = tempBuffer;
            }

            player.GetFilteredSpectrumData(buffer, tempBuffer, smoothAmount, window);
        }
        /// <summary>
        /// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
        /// </summary>
        /// <param name="player">Плеер из которого берутся данные спектра</param>
        /// <param name="buffer">Буфер в который будут записаны итоговые данные спектра</param>
        /// <param name="tempBuffer">Буфер в который будут записываться временные данные спектра. Длина этого массива должна быть точно такой же как и у <paramref name="buffer"/></param>
        /// <param name="smoothAmount">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <param name="window">FFT окно</param>
        /// <returns>Готовые к использованию данные спектра</returns>
        /// <exception cref="ArgumentNullException">Ссылка на плеер не указывает на его экземпляр</exception>
        /// <exception cref="ArgumentException">Степень сглаживания меньше нуля</exception>
        public static float[] GetFilteredSpectrumData(this ISimplePlayer player, float[] buffer, float[] tempBuffer, int smoothAmount, SpectrumWindow window = SpectrumWindow.Rectangular)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player), nameof(player) + " не указан");
            }
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            if (tempBuffer == null)
            {
                throw new ArgumentNullException(nameof(tempBuffer));
            }
            if (smoothAmount < 0)
            {
                throw new ArgumentException("Сглаживание не может быть отрицательным");
            }

            player.GetFilteredSpectrumData(tempBuffer, window);
            ArrayWork.Smooth(tempBuffer, buffer, smoothAmount);

            float average = ArrayWork.Average(buffer) * 2;
            ArrayWork.Multiply(buffer, average);

            return buffer;
        }
    }
}
