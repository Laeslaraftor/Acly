namespace Acly.Numbers
{
    /// <summary>
    /// Обновление кадра анимации
    /// </summary>
    /// <param name="animation">Анимация, которая вызвала событие</param>
    /// <param name="value">Значение кадра</param>
    public delegate void ValueAnimationTick(ValueAnimation animation, float value);
}
