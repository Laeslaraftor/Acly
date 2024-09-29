using Acly.Requests;
using System.IO.Ports;

namespace Acly.Usb
{
    /// <summary>
    /// Событие начала получения данных с Usb устройства
    /// </summary>
    /// <param name="Listener">Читатель, вызвавший событие</param>
    /// <param name="Device">Устройство, присылающее данные</param>
    /// <param name="Size">Размер получаемых данных</param>
    public delegate void UsbListenerDeviceStartDataReceivingEvent(IListener Listener, IUsbPort Device, int Size);
}
