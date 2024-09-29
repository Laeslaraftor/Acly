using Acly.Requests;
using System.IO.Ports;

namespace Acly.Usb
{
    /// <summary>
    /// Событие устройства Usb читателя
    /// </summary>
    /// <param name="Listener">Читатель, вызвавший событие</param>
    /// <param name="Device">Устройство события</param>
    public delegate void UsbListenerDeviceEvent(IListener Listener, IUsbPort Device);
}
