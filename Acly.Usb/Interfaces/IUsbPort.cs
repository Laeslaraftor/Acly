using System;

namespace Acly.Usb
{
    /// <summary>
    /// Интерфейс подключения Usb порта
    /// </summary>
    public interface IUsbPort : IDisposable
    {
        /// <summary>
        /// Вызывается при отключении устройства
        /// </summary>
        public event UsbPortEvent? Disconnected;
        /// <summary>
        /// Вызывается при получении данных
        /// </summary>
        public event UsbPortEvent? DataReceived;

        /// <summary>
        /// Название порта
        /// </summary>
        public string PortName { get; }
        /// <summary>
        /// Количество доступных для чтения данных
        /// </summary>
        public int BytesToRead { get; }
        /// <summary>
        /// Время ожидания отклика
        /// </summary>
        public TimeSpan ResponseTimeout { get; set; }

        #region Управление

        /// <summary>
        /// Отправить данные
        /// </summary>
        /// <param name="Data">Данные для отправки</param>
        /// <param name="Offset">Смещение</param>
        /// <param name="Count">Длина</param>
        public void Write(byte[] Data, int Offset, int Count);
        /// <summary>
        /// Прочитать данные
        /// </summary>
        /// <param name="Data">Массив в который будут записаны данные</param>
        /// <param name="Offset">Смещение</param>
        /// <param name="Count">Длина</param>
        /// <returns>Количество прочитанных данных</returns>
        public int Read(byte[] Data, int Offset, int Count);
        /// <summary>
        /// Прочитать данные и конвертировать в строку
        /// </summary>
        /// <returns>Полученная строка</returns>
        public string ReadLine();

        #endregion
    }
}
