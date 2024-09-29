using System;
using System.IO.Ports;
using System.Text;

namespace Acly.Usb
{
    /// <summary>
    /// Класс методов расширений
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Отправить закодированную строку
        /// </summary>
        /// <param name="Port">Порт, которому будет отправлена строка</param>
        /// <param name="Value">Строка для отправки</param>
        /// <param name="Encoding">Кодировка</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Write(this IUsbPort Port, string Value, Encoding Encoding)
        {
            if (Port == null)
            {
                throw new ArgumentNullException(nameof(Port));
            }
            if (Encoding == null)
            {
                throw new ArgumentNullException(nameof(Encoding));
            }

            byte[] EncodedValue = Encoding.GetBytes(Value);
            Port.Write(EncodedValue, 0, EncodedValue.Length);
        }
    }
}
