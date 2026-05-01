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
        public static RuntimePlatform Current
        {
            get
            {
                _current ??= DetectPlatform();
                return _current.Value;
            }
        }

        private static RuntimePlatform? _current;

        private static RuntimePlatform DetectPlatform()
        {
            string os = Environment.OSVersion.ToString();
            IEnumerable<string> platforms = Enum.GetNames(typeof(RuntimePlatform));
            bool isArmProcessor = RuntimeInformation.ProcessArchitecture == Architecture.Arm || RuntimeInformation.ProcessArchitecture == Architecture.Arm64;

            foreach (var platform in platforms)
            {
                Match search = Regex.Match(os, platform.ToString());

                if (search.Success)
                {
                    return Enum.Parse<RuntimePlatform>(platform);
                }
            }

            if (Environment.OSVersion.Platform == PlatformID.Unix && isArmProcessor)
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
