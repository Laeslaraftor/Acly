using System;

namespace Acly.Platforms
{
	/// <summary>
	/// Платформа, на которой выполняется приложение
	/// </summary>
	[Flags]
	public enum RuntimePlatform
	{
#pragma warning disable CS1591
		Windows = 1,
		Android = 2,
		IOS = 4,
		MacOS = 8,
		Linux = 16,
		Unknown = 32
#pragma warning restore CS1591
	}
}
