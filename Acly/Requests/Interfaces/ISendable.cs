namespace Acly.Requests
{
    /// <summary>
    /// Возможность отправки данных
    /// </summary>
    public interface ISendable
    {
        /// <summary>
        /// Отправить данные
        /// </summary>
        /// <param name="data">Данные для отправки</param>
        /// <param name="offset">Смещение</param>
        /// <param name="length">Длина записи</param>
        public void Send(byte[] data, int offset, int length);

        /// <summary>
        /// Отправить объект
        /// </summary>
        /// <param name="obj">Объект для отправки</param>
        public void Send(object obj);
    }
}
