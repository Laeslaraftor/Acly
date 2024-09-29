namespace Acly.Usb
{
    /// <summary>
    /// Коды результата отправки данных устройству
    /// </summary>
    public enum UsbDataSendResult
    {
        /// <summary>
        /// Устройство отказало в отправке
        /// </summary>
        DeviceDenied,
        /// <summary>
        /// Устройство отказалось от приёма данных
        /// </summary>
        DataDenied,
        /// <summary>
        /// Данные успешно отправлены
        /// </summary>
        Success
    }
}
