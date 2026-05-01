namespace Acly.Player
{
    /// <summary>
    /// Событие изменения состояния <see cref="ISimplePlayer"/>
    /// </summary>
    /// <param name="player">Плеер, вызвавший событие</param>
    /// <param name="state">Новое состояние плеера</param>
    public delegate void SimplePlayerStateEvent(ISimplePlayer player, SimplePlayerState state);
}
