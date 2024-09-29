using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Acly.Platforms
{
	/// <summary>
	/// Класс для работы с текущей платформой
	/// </summary>
	public static class Platform
	{
		/// <summary>
		/// Текущая платформа, на которой выполняется приложение
		/// </summary>
		public static RuntimePlatform Current => DetectPlatform();

		private static RuntimePlatform DetectPlatform()
		{
			string OS = Environment.OSVersion.ToString();
			IEnumerable<string> Platforms = Enum.GetNames(typeof(RuntimePlatform));
			bool IsArmProcessor = RuntimeInformation.ProcessArchitecture == Architecture.Arm || RuntimeInformation.ProcessArchitecture == Architecture.Arm64;

			foreach (var Platform in Platforms)
			{
				Match Search = Regex.Match(OS, Platform.ToString());

				if (Search.Success)
				{
					return Enum.Parse<RuntimePlatform>(Platform);
				}
			}

			if (Environment.OSVersion.Platform == PlatformID.Unix && IsArmProcessor)
			{
				return RuntimePlatform.Android;
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				return RuntimePlatform.Linux;
			}
			else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
			{
				return RuntimePlatform.MacOS;
			}
			else if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				return RuntimePlatform.IOS;
			}

			return RuntimePlatform.Unknown;
		}
	}
}
