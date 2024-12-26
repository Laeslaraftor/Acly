using System;

namespace Acly.Player.Extensions
{
    /// <summary>
    /// Класс с методами расширения для плеера
    /// </summary>
    public static class PlayerExtensions
    {
        /// <summary>
        /// Переключить состояние плеера воспроизвести/пауза
        /// </summary>
        /// <param name="Player">Плеер, состояние которого будет изменяться</param>
        /// <returns>Новое состояние плеера</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static SimplePlayerState SwitchState(this ISimplePlayer Player)
        {
            if (Player == null)
            {
                throw new ArgumentNullException(nameof(Player), "Плеер не указан");
            }

            if (Player.State != SimplePlayerState.Playing)
            {
                Player.Play();
                return Player.State;
            }

            Player.Pause();

            return Player.State;
        }
    }
}
