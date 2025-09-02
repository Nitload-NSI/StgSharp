using System;

namespace StgSharp
{
    /// <summary>
    /// Test World class for multi-version framework demonstration
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

            Console.WriteLine($"[StgSharp Dev v{version / 100.0:F1}] Framework initialized directly");
            _initialized = true;
        }

        /// <summary>
        /// Run the framework
        /// </summary>
        public static void Run()
        {
            if (!_initialized)
                Init();

            Console.WriteLine($"[StgSharp Dev v{version / 100.0:F1}] Framework running directly");
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public static void LogInfo(string message)
        {
            Console.WriteLine($"[DEV-INFO] {message}");
        }

        /// <summary>
        /// Log error
        /// </summary>
        public static void LogError(Exception exception)
        {
            Console.WriteLine($"[DEV-ERROR] {exception.Message}");
        }
    }
}