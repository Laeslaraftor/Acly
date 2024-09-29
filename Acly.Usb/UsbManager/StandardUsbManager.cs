using System;
using System.IO.Ports;

namespace Acly.Usb
{
    /// <summary>
    /// Стандартный менеджер Usb портов, основанный на <see cref="SerialPort"/>
    /// </summary>
    public sealed class StandardUsbManager : IUsbManager
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public string[] GetPortNames() => SerialPort.GetPortNames();
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="Name"><inheritdoc/></param>
        /// <returns><inheritdoc/></returns>
        public IUsbPort GetPort(string Name) => new StandardUsbPort(Name);
    }
}
