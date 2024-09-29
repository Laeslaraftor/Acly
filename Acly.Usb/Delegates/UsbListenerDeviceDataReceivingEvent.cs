using Acly.Requests;
using System.IO.Ports;

namespace Acly.Usb
{
    /// <summary>
    /// Событие прогресса получения данных с Usb устройства
    /// </summary>
    /// <param name="Listener">Читатель, вызвавший событие</param>
    /// <param name="Device">Устройство, присылающее данные</param>
    /// <param name="Received">Размер уже полученных данных</param>
    /// <param name="Total">Размер данных для получения</param>
    public delegate void UsbListenerDeviceDataReceivingEvent(IListener Listener, IUsbPort Device, int Received, int Total);
}
