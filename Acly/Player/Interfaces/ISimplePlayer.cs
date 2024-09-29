using System;
using System.IO;
using System.Threading.Tasks;

namespace Acly.Player
{
	/// <summary>
	/// Интерфейс реализации SimplePlayer
	/// </summary>
	public interface ISimplePlayer
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
		/// Проигрывается ли сейчас аудио
		/// </summary>
		public bool IsPlaying { get; }
		/// <summary>
		/// Установлен ли источник
		/// </summary>
		public bool SourceSetted { get; }
		/// <summary>
		/// Автовоспроизведение аудио после его смены
		/// </summary>
		public bool AutoPlay { get; set; }
		/// <summary>
		/// Текущее состояние плеера
		/// </summary>
		public SimplePlayerState State { get; }

		#region Установка

		/// <summary>
		/// Установить источник из массива байтов аудиофайла
		/// </summary>
		/// <param name="Data">Массив байтов аудиофайла</param>
		public Task SetSource(byte[] Data);
		/// <summary>
		/// Установить источник из трансляции аудиофайла
		/// </summary>
		/// <param name="SourceStream">Трансляция аудиофайла</param>
		public Task SetSource(Stream SourceStream);
        /// <summary>
        /// Установить источник из URL адреса
        /// </summary>
        /// <param name="SourceUrl">URL адрес аудио</param>
        public Task SetSource(string SourceUrl);

        #endregion

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
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Данные спектра</returns>
		public float[] GetSpectrumData(int Size, SpectrumWindow Window = SpectrumWindow.Rectangular);
		/// <summary>
		/// Получить данные спектра указанного размера со сглаживанием и применением указанного FFT окна
		/// </summary>
		/// <param name="Size">Размер требуемых данных спектра</param>
		/// <param name="SmoothAmount">Степень сглаживания. 
		/// Например, пусть степень сглаживания будет равна 2, то значение arr[i] будет равно среднему арифметическому значений от arr[i - 2] до arr[i + 2] включительно
		/// </param>
		/// <param name="Window">FFT окно</param>
		/// <returns>Данные спектра со сглаживанием</returns>
		public float[] GetSpectrumData(int Size, int SmoothAmount, SpectrumWindow Window = SpectrumWindow.Rectangular);

		#endregion

		#region Очистка

		/// <summary>
		/// Освободить ресурсы, используемые плеером. После освобождения плеер невозможно использовать
		/// </summary>
		public void Release();

		#endregion
	}
}
