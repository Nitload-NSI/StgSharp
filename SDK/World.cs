using System;
using StgSharp.SDK.Loaders;

namespace StgSharp.SDK
{
    /// <summary>
    /// Main entry point for the StgSharp SDK - provides simple World.Run() API
    /// </summary>
    public static class World
    {
        private static IFrameworkLoader? _loader;
        private static IFrameworkRuntime? _runtime;
        private static readonly object _lock = new object();

        /// <summary>
        /// Initialize the SDK with specified version or auto-detect
        /// </summary>
        /// <param name="version">Specific version to use, or null for auto-detection</param>
        public static void Init(string? version = null)
        {
            lock (_lock)
            {
                if (_runtime != null)
                    return;

                _loader ??= new DynamicFrameworkLoader();
                
                var targetVersion = version ?? GetConfiguredVersion() ?? _loader.GetDefaultVersion();
                _runtime = _loader.LoadFramework(targetVersion);
                
                _runtime.World.Init();
            }
        }

        /// <summary>
        /// Simple run method - main entry point for applications
        /// </summary>
        public static void Run()
        {
            lock (_lock)
            {
                if (_runtime == null)
                    Init();
                
                _runtime?.World.Run();
            }
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public static void LogInfo(string message)
        {
            lock (_lock)
            {
                if (_runtime == null)
                    Init();
                
                _runtime?.World.LogInfo(message);
            }
        }

        /// <summary>
        /// Log error
        /// </summary>
        public static void LogError(Exception exception)
        {
            lock (_lock)
            {
                if (_runtime == null)
                    Init();
                
                _runtime?.World.LogError(exception);
            }
        }

        /// <summary>
        /// Get current framework version information
        /// </summary>
        public static IFrameworkVersion? GetCurrentVersion()
        {
            return _runtime?.Version;
        }

        /// <summary>
        /// Get all available framework versions
        /// </summary>
        public static IFrameworkVersion[] GetAvailableVersions()
        {
            _loader ??= new DynamicFrameworkLoader();
            return _loader.GetAvailableVersions();
        }

        /// <summary>
        /// Switch to a different framework version (disposes current runtime)
        /// </summary>
        public static void SwitchVersion(string version)
        {
            lock (_lock)
            {
                _runtime?.Dispose();
                _runtime = null;
                Init(version);
            }
        }

        /// <summary>
        /// Get configured version from environment or config files
        /// </summary>
        private static string? GetConfiguredVersion()
        {
            // Check environment variable first
            var envVersion = Environment.GetEnvironmentVariable("STGSHARP_VERSION");
            if (!string.IsNullOrEmpty(envVersion))
                return envVersion;

            // Check for stgsharp.config.json
            try
            {
                var configPath = "stgsharp.config.json";
                if (System.IO.File.Exists(configPath))
                {
                    var json = System.IO.File.ReadAllText(configPath);
                    // Simple JSON parsing for version field
                    var versionIndex = json.IndexOf("\"version\"");
                    if (versionIndex != -1)
                    {
                        var start = json.IndexOf(':', versionIndex) + 1;
                        var end = json.IndexOf(',', start);
                        if (end == -1) end = json.IndexOf('}', start);
                        if (start > 0 && end > start)
                        {
                            return json.Substring(start, end - start)
                                      .Trim(' ', '"', '\r', '\n', '\t');
                        }
                    }
                }
            }
            catch
            {
                // Ignore config file errors
            }

            return null;
        }

        /// <summary>
        /// Cleanup resources when application exits
        /// </summary>
        public static void Dispose()
        {
            lock (_lock)
            {
                _runtime?.Dispose();
                _runtime = null;
            }
        }
    }
}