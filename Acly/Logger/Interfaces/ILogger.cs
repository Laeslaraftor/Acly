namespace Acly
{
    /// <summary>
    /// Интерфейс отправителя сообщений
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Отправить объект как сообщение
        /// </summary>
        /// <param name="obj">Объект для сообщения</param>
        public void Message(object obj);

        /// <summary>
        /// Отправить объект как предупреждение
        /// </summary>
        /// <param name="obj">Объект для предупреждения</param>
        public void Warning(object obj);

        /// <summary>
        /// Отправить объект как сообщение об ошибке
        /// </summary>
        /// <param name="obj">Объект для сообщения об ошибке</param>
        public void Error(object obj);
    }
}
