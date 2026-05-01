using System;
using System.Threading.Tasks;

namespace Acly.Player
{
    /// <summary>
    /// Интерфейс реализации SimplePlayer
    /// </summary>
    public interface ISimplePlayer : IDisposable
    {
        /// <summary>
        /// Вызывается при изменении состояния плеера
        /// </summary>
        public event SimplePlayerStateEvent? StateChanged;
        /// <summary>
        /// Вызывается при изменении источника
        /// </summary>
        public event SimplePlayerEvent? SourceChanged;
        /// <summary>
        /// Вызывается при окончании аудио
        /// </summary>
        public event SimplePlayerEvent? SourceEnded;

        /// <summary>
        /// Текущее время проигрывания
        /// </summary>
        public TimeSpan Position { get; set; }
        /// <summary>
        /// Продолжительность аудио
        /// </summary>
        public TimeSpan Duration { get; }
        /// <summary>
        /// Скорость воспроизведения
        /// </summary>
        public float Speed { get; set; }
        /// <summary>
        /// Громкость плеера
        /// </summary>
        public float Volume { get; set; }
        /// <summary>
        /// Повторение аудио
        /// </summary>
        public bool Loop { get; set; }
        /// <summary>
        /// Автовоспроизведение аудио после его смены
        /// </summary>
        public bool AutoPlay { get; set; }
        /// <summary>
        /// Текущее состояние плеера
        /// </summary>
        public SimplePlayerState State { get; }
        /// <summary>
        /// Текущий источник аудио
        /// </summary>
        public object? Source { get; }

        #region Управление

        /// <summary>
        /// Воспроизвести / продолжить
        /// </summary>
        public void Play();
        /// <summary>
        /// Поставить на паузу
        /// </summary>
        public void Pause();
        /// <summary>
        /// Остановить воспроизведение и сбросить текущую позицию
        /// </summary>
        public void Stop();

        /// <summary>
        /// Получить данные спектра указанного размера с применением указанного FFT окна
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра</returns>
        public float[] GetSpectrumData(int size, SpectrumWindow window = SpectrumWindow.Rectangular);
        /// <summary>
        /// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
        /// </summary>
        /// <param name="size">Размер требуемых данных спектра</param>
        /// <param name="smoothAmount">Степень сглаживания. 
        /// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
        /// </param>
        /// <param name="window">FFT окно</param>
        /// <returns>Данные спектра со сглаживанием</returns>
        public float[] GetSpectrumData(int size, int smoothAmount, SpectrumWindow window = SpectrumWindow.Rectangular);

        #endregion
    }
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public interface ISimplePlayer<T> : ISimplePlayer
    {
        #region Установка

        /// <summary>
        /// Установить источник из аудио
        /// </summary>
        /// <param name="source">Источник аудио</param>
        public Task SetSource(T source);

        #endregion
    }
}
