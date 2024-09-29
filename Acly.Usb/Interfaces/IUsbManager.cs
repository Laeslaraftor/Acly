namespace Acly.Usb
{
    /// <summary>
    /// Интерфейс менеджера Usb портов
    /// </summary>
    public interface IUsbManager
    {
        /// <summary>
        /// Получить названия доступных Usb портов
        /// </summary>
        /// <returns>Список доступных Usb портов</returns>
        public string[] GetPortNames();
        /// <summary>
        /// Получить подключение к Usb порту
        /// </summary>
        /// <param name="Name">Название Usb порта</param>
        /// <returns>Подключение к Usb порту</returns>
        public IUsbPort GetPort(string Name);
    }
}
