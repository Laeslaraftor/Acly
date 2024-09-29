namespace Acly.Player
{
    /// <summary>
    /// Событие изменения состояния <see cref="ISimplePlayer"/>
    /// </summary>
    /// <param name="Player">Плеер, вызвавший событие</param>
    /// <param name="State">Новое состояние плеера</param>
    public delegate void SimplePlayerStateEvent(ISimplePlayer Player, SimplePlayerState State);
}
