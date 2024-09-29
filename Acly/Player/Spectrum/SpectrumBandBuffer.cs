using System;

namespace Acly.Player
{
    internal static class SpectrumBandBuffer
    {
        private static float[]? _Buffer;
        private static float[]? _BufferDecrease;

        public static float[] ApplyBuffer(float[] Samples, float DecreaseSize = 0.005f, float DecreaseMultiplier = 1.2f)
        {
            if (_Buffer == null || Samples.Length != _Buffer.Length || _BufferDecrease == null)
            {
                _Buffer = Samples;
                _BufferDecrease = new float[Samples.Length];
                return Samples;
            }

            for (int i = 0; i < Samples.Length; i++)
            {
                if (Samples[i] > _Buffer[i])
                {
                    _Buffer[i] = Samples[i];
                    _BufferDecrease[i] = DecreaseSize;
                }
                else if (Samples[i] < _Buffer[i])
                {
                    _Buffer[i] = MathF.Max(_Buffer[i] - _BufferDecrease[i], 0);
                    _BufferDecrease[i] *= DecreaseMultiplier;
                }
            }

            return _Buffer;
        }
    }
}
