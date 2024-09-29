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
		/// <param name="Data">Данные для отправки</param>
		/// <param name="Offset">Смещение</param>
		/// <param name="Length">Длина записи</param>
		public void Send(byte[] Data, int Offset, int Length);

        /// <summary>
        /// Отправить объект
        /// </summary>
        /// <param name="Object">Объект для отправки</param>
        public void Send(object Object);
    }
}
