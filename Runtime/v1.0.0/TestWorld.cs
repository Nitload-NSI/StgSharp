using System;

namespace StgSharp
{
    /// <summary>
    /// Simplified World class for testing multi-version framework
    /// </summary>
    public static class World
    {
        /// <summary>
        /// Version of current World platform
        /// </summary>
        public const long version = 100; // v1.0.0

        private static bool _initialized = false;

        /// <summary>
        /// Initialize the framework
        /// </summary>
        public static void Init()
        {
            if (_initialized)
                return;

            Console.WriteLine($"[StgSharp v{version / 100.0:F1}] Framework initialized");
            _initialized = true;
        }

        /// <summary>
        /// Run the framework
        /// </summary>
        public static void Run()
        {
            if (!_initialized)
                Init();

            Console.WriteLine($"[StgSharp v{version / 100.0:F1}] Framework running");
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public static void LogInfo(string message)
        {
            Console.WriteLine($"[INFO] {message}");
        }

        /// <summary>
        /// Log error
        /// </summary>
        public static void LogError(Exception exception)
        {
            Console.WriteLine($"[ERROR] {exception.Message}");
        }
    }
}