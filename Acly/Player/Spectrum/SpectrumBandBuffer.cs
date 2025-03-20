using System;

namespace Acly.Player
{
    /// <summary>
    /// Работа со спертром
    /// </summary>
    public static class SpectrumBandBuffer
    {
        private static float[]? _Buffer;
        private static float[]? _BufferDecrease;

        /// <summary>
        /// Применить буфер для массива. Применяя этот буфер, значения, находящиеся в массиве уменьшаются, пока не станут равны нулю.
        /// Чем дольше значения уменьшаются тем сильнее начинают уменьшаться. С каждым уменьшением скорость умножается на множитель уменьшения.
        /// </summary>
        /// <param name="Samples">Массив для уменьшения</param>
        /// <param name="DecreaseSize">Начальная скорость уменьшения</param>
        /// <param name="DecreaseMultiplier">Множитель уменьшения</param>
        /// <returns>Плавно уменьшенный буфер</returns>
        public static float[] ApplyBuffer(float[] Samples, float DecreaseSize = 0.005f, float DecreaseMultiplier = 1.2f)
        {
            return ApplyBuffer(Samples, ref _Buffer, ref _BufferDecrease, DecreaseSize, DecreaseMultiplier);
        }
        /// <summary>
        /// Применить буфер для массива. Применяя этот буфер, значения, находящиеся в массиве уменьшаются, пока не станут равны нулю.
        /// Чем дольше значения уменьшаются тем сильнее начинают уменьшаться. С каждым уменьшением скорость умножается на множитель уменьшения.
        /// </summary>
        /// <param name="Samples">Массив для уменьшения</param>
        /// <param name="Buffer">Буфер</param>
        /// <param name="BufferDecrease">Буфер уменьшения</param>
        /// <param name="DecreaseSize">Начальная скорость уменьшения</param>
        /// <param name="DecreaseMultiplier">Множитель уменьшения</param>
        /// <returns>Плавно уменьшенный буфер</returns>
        public static float[] ApplyBuffer(float[] Samples, ref float[]? Buffer, ref float[]? BufferDecrease, float DecreaseSize = 0.005f, float DecreaseMultiplier = 1.2f)
        {
            if (Buffer == null || Samples.Length != Buffer.Length || BufferDecrease == null)
            {
                Buffer = Samples;
                BufferDecrease = new float[Samples.Length];
                return Samples;
            }

            for (int i = 0; i < Samples.Length; i++)
            {
                if (Samples[i] > Buffer[i])
                {
                    Buffer[i] = Samples[i];
                    BufferDecrease[i] = DecreaseSize;
                }
                else if (Samples[i] < Buffer[i])
                {
                    Buffer[i] = MathF.Max(Buffer[i] - BufferDecrease[i], 0);
                    BufferDecrease[i] *= DecreaseMultiplier;
                }
            }

            return Buffer;
        }
    }
}
