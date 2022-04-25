using System;
using System.Runtime.InteropServices;
using MoralisUnity.Platform.Abstractions;

namespace MoralisUnity.Platform
{
    /// <summary>
    /// Inferred data about the environment in which Moralis is operating.
    /// </summary>
    public class EnvironmentData : IEnvironmentData
    {
        /// <summary>
        /// A <see cref="EnvironmentData"/> instance that the Moralis SDK will attempt to generate from environment metadata it should be able to access.
        /// </summary>
        public static EnvironmentData Inferred => new EnvironmentData
        {
            TimeZone = TimeZoneInfo.Local.StandardName,
            #if UNITY_WEBGL
            OSVersion = "unknown", // WebGL doesn't know the OS
            Platform = ".NET"
            #else
            OSVersion = RuntimeInformation.OSDescription ?? Environment.OSVersion.ToString(),
            Platform = RuntimeInformation.FrameworkDescription ?? ".NET"
            #endif
        };

        /// <summary>
        /// The active time zone for the app and/or system.
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// The host operating system version of the platform the host application is operating in.
        /// </summary>
        public string OSVersion { get; set; }

        /// <summary>
        /// The target platform the app is running on. Defaults to .NET.
        /// </summary>
        public string Platform { get; set; }
    }

}
