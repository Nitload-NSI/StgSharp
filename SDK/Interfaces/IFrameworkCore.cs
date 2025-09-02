using System;

namespace StgSharp.SDK
{
    /// <summary>
    /// Core interface for framework version information
    /// </summary>
    public interface IFrameworkVersion
    {
        /// <summary>
        /// Framework version string (e.g., "1.0.0")
        /// </summary>
        string Version { get; }
        
        /// <summary>
        /// Framework compatibility level
        /// </summary>
        long CompatibilityLevel { get; }
        
        /// <summary>
        /// Framework release type (Alpha, Beta, Release)
        /// </summary>
        string ReleaseType { get; }
    }

    /// <summary>
    /// Core interface for framework runtime loader
    /// </summary>
    public interface IFrameworkLoader
    {
        /// <summary>
        /// Load a specific framework version
        /// </summary>
        /// <param name="version">Version to load</param>
        /// <returns>Loaded framework instance</returns>
        IFrameworkRuntime LoadFramework(string version);
        
        /// <summary>
        /// Get available framework versions
        /// </summary>
        IFrameworkVersion[] GetAvailableVersions();
        
        /// <summary>
        /// Get default framework version
        /// </summary>
        string GetDefaultVersion();
    }

    /// <summary>
    /// Core interface for framework runtime instance
    /// </summary>
    public interface IFrameworkRuntime : IDisposable
    {
        /// <summary>
        /// Framework version information
        /// </summary>
        IFrameworkVersion Version { get; }
        
        /// <summary>
        /// World API proxy for this runtime version
        /// </summary>
        IWorldProxy World { get; }
    }

    /// <summary>
    /// Core interface for World API proxy
    /// </summary>
    public interface IWorldProxy
    {
        /// <summary>
        /// Initialize the framework
        /// </summary>
        void Init();
        
        /// <summary>
        /// Run the framework
        /// </summary>
        void Run();
        
        /// <summary>
        /// Log information
        /// </summary>
        void LogInfo(string message);
        
        /// <summary>
        /// Log error
        /// </summary>
        void LogError(Exception exception);
    }
}