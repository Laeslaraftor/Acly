using Acly.Requests;
using System.IO.Ports;

namespace Acly.Usb
{
    /// <summary>
    /// Событие получения данных с Usb устройства
    /// </summary>
    /// <param name="Listener">Читатель, вызвавший событие</param>
    /// <param name="Device">Устройство, приславшее данные</param>
    /// <param name="Data">Полученные данные</param>
    public delegate void UsbListenerDeviceDataEvent(IListener Listener, IUsbPort Device, byte[] Data);
}
