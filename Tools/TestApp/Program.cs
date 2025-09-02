using System;

#if STGSHARP_DEVELOPMENT
// Development mode - direct framework usage
using StgSharp;
#else
// Release mode - SDK usage  
using StgSharp.SDK;
#endif

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("StgSharp Multi-Version Framework Test");
            Console.WriteLine("=====================================");

#if STGSHARP_DEVELOPMENT
            Console.WriteLine("Running in DEVELOPMENT mode");
            Console.WriteLine("- Direct framework reference");
            Console.WriteLine("- Full IntelliSense support");
            Console.WriteLine("- Source-level debugging");
#else
            Console.WriteLine("Running in RELEASE mode");
            Console.WriteLine("- SDK dynamic loading");
            Console.WriteLine("- Version isolation");
            Console.WriteLine("- Runtime version selection");
#endif

            try
            {
                // This will be intercepted by source generator
                World.LogInfo("Framework initialization starting...");
                
                // Initialize the framework
                World.Init();
                
                World.LogInfo("Framework initialized successfully!");
                
                // Get version information in release mode
#if !STGSHARP_DEVELOPMENT
                var version = World.GetCurrentVersion();
                if (version != null)
                {
                    Console.WriteLine($"Loaded Framework Version: {version.Version}");
                    Console.WriteLine($"Release Type: {version.ReleaseType}");
                }

                var availableVersions = World.GetAvailableVersions();
                Console.WriteLine($"Available Versions: {availableVersions.Length}");
                foreach (var v in availableVersions)
                {
                    Console.WriteLine($"  - {v.Version} ({v.ReleaseType})");
                }
#endif

                Console.WriteLine("\nFramework test completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                World.LogError(ex);
            }
        }
    }
}