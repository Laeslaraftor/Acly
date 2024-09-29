namespace Acly.Player
{
	/// <summary>
	/// Типы FFT окна
	/// </summary>
	public enum SpectrumWindow
	{
		/// <summary>
		/// W[n] = 1.0
		/// </summary>  
		Rectangular,
		/// <summary>
		/// W[n] = TRI(2n/N)
		/// </summary>
		Triangle,
		/// <summary>
		/// W[n] = 0.54 - (0.46 * COS(n/N) )
		/// </summary>
		Hamming,
		/// <summary>
		/// W[n] = 0.5 * (1.0 - COS(n/N) )
		/// </summary>
		Hanning,
		/// <summary>
		/// W[n] = 0.42 - (0.5 * COS(nN) ) + (0.08 * COS(2.0 * nN) )
		/// </summary>
		Blackman,
		/// <summary>
		/// W[n] = 0.35875 - (0.48829 * COS(1.0 * nN)) + (0.14128 * COS(2.0 * nN)) - (0.01168 * COS(3.0 * n/N))
		/// </summary>
		BlackmanHarris
	}
}
