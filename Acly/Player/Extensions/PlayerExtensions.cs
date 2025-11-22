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
        /// <summary>
        /// Скопировать значения основных параметров из другого плеера
        /// </summary>
        /// <param name="Player">Плеер который получит значения из другого плеера</param>
        /// <param name="Reference">Плеер, значения которого будут использоваться</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CopyValues(this ISimplePlayer Player, ISimplePlayer Reference)
        {
            if (Player == null)
            {
                throw new ArgumentNullException(nameof(Player));
            }
            if (Reference == null)
            {
                throw new ArgumentNullException(nameof(Player));
            }

            Player.AutoPlay = Reference.AutoPlay;
            Player.Loop = Reference.Loop;
            Player.Speed = Reference.Speed;
            Player.Volume = Reference.Volume;
        }
    }
}
