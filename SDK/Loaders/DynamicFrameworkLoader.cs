using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Collections.Generic;
using System.Linq;

namespace StgSharp.SDK.Loaders
{
    /// <summary>
    /// Dynamic framework loader using AssemblyLoadContext
    /// </summary>
    public class DynamicFrameworkLoader : IFrameworkLoader
    {
        private readonly string _runtimePath;
        private readonly Dictionary<string, IFrameworkRuntime> _loadedRuntimes;

        public DynamicFrameworkLoader(string? runtimePath = null)
        {
            _runtimePath = runtimePath ?? GetDefaultRuntimePath();
            _loadedRuntimes = new Dictionary<string, IFrameworkRuntime>();
        }

        private static string GetDefaultRuntimePath()
        {
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";
            
            // Try different possible locations for Runtime directory
            var possiblePaths = new[]
            {
                Path.Combine(baseDir, "Runtime"),           // Same directory as executable
                Path.Combine(baseDir, "..", "Runtime"),     // Parent directory
                Path.Combine(baseDir, "..", "..", "Runtime"), // Two levels up
                "Runtime"                                   // Current working directory
            };

            foreach (var path in possiblePaths)
            {
                if (Directory.Exists(path))
                    return path;
            }

            // Default to same directory as executable
            return Path.Combine(baseDir, "Runtime");
        }

        public IFrameworkRuntime LoadFramework(string version)
        {
            if (_loadedRuntimes.TryGetValue(version, out var cached))
                return cached;

            var versionPath = Path.Combine(_runtimePath, $"v{version}");
            if (!Directory.Exists(versionPath))
                throw new DirectoryNotFoundException($"Framework version {version} not found at {versionPath}");

            var assemblyPath = Path.Combine(versionPath, "StgSharp.Common.dll");
            if (!File.Exists(assemblyPath))
                throw new FileNotFoundException($"Framework assembly not found at {assemblyPath}");

            var loadContext = new FrameworkLoadContext(versionPath);
            var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
            
            var runtime = new DynamicFrameworkRuntime(assembly, version, loadContext);
            _loadedRuntimes[version] = runtime;
            
            return runtime;
        }

        public IFrameworkVersion[] GetAvailableVersions()
        {
            if (!Directory.Exists(_runtimePath))
                return Array.Empty<IFrameworkVersion>();

            var versions = new List<IFrameworkVersion>();
            foreach (var dir in Directory.GetDirectories(_runtimePath))
            {
                var dirName = Path.GetFileName(dir);
                if (dirName.StartsWith("v") && dirName.Length > 1)
                {
                    var version = dirName.Substring(1);
                    versions.Add(new FrameworkVersion(version, 0, "Release"));
                }
            }
            
            return versions.ToArray();
        }

        public string GetDefaultVersion()
        {
            var versions = GetAvailableVersions();
            return versions.LastOrDefault()?.Version ?? "1.0.0";
        }
    }

    /// <summary>
    /// Custom AssemblyLoadContext for framework isolation
    /// </summary>
    internal class FrameworkLoadContext : AssemblyLoadContext
    {
        private readonly string _frameworkPath;

        public FrameworkLoadContext(string frameworkPath) : base(isCollectible: true)
        {
            _frameworkPath = frameworkPath;
        }

        protected override Assembly? Load(AssemblyName assemblyName)
        {
            var assemblyPath = Path.Combine(_frameworkPath, $"{assemblyName.Name}.dll");
            if (File.Exists(assemblyPath))
            {
                return LoadFromAssemblyPath(assemblyPath);
            }
            
            return null;
        }
    }

    /// <summary>
    /// Framework version information
    /// </summary>
    internal class FrameworkVersion : IFrameworkVersion
    {
        public string Version { get; }
        public long CompatibilityLevel { get; }
        public string ReleaseType { get; }

        public FrameworkVersion(string version, long compatibilityLevel, string releaseType)
        {
            Version = version;
            CompatibilityLevel = compatibilityLevel;
            ReleaseType = releaseType;
        }
    }

    /// <summary>
    /// Dynamic framework runtime instance
    /// </summary>
    internal class DynamicFrameworkRuntime : IFrameworkRuntime
    {
        private readonly Assembly _assembly;
        private readonly string _version;
        private readonly FrameworkLoadContext _loadContext;
        private IWorldProxy? _worldProxy;

        public IFrameworkVersion Version { get; }
        
        public IWorldProxy World => _worldProxy ??= CreateWorldProxy();

        public DynamicFrameworkRuntime(Assembly assembly, string version, FrameworkLoadContext loadContext)
        {
            _assembly = assembly;
            _version = version;
            _loadContext = loadContext;
            Version = new FrameworkVersion(version, 0, "Release");
        }

        private IWorldProxy CreateWorldProxy()
        {
            return new DynamicWorldProxy(_assembly);
        }

        public void Dispose()
        {
            _loadContext?.Unload();
        }
    }

    /// <summary>
    /// Dynamic World API proxy using reflection
    /// </summary>
    internal class DynamicWorldProxy : IWorldProxy
    {
        private readonly Assembly _assembly;
        private readonly Type? _worldType;
        private readonly MethodInfo? _initMethod;
        private readonly MethodInfo? _runMethod;
        private readonly MethodInfo? _logInfoMethod;
        private readonly MethodInfo? _logErrorMethod;

        public DynamicWorldProxy(Assembly assembly)
        {
            _assembly = assembly;
            _worldType = assembly.GetType("StgSharp.World");
            
            if (_worldType != null)
            {
                _initMethod = _worldType.GetMethod("Init", BindingFlags.Public | BindingFlags.Static);
                _runMethod = _worldType.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);
                _logInfoMethod = _worldType.GetMethod("LogInfo", BindingFlags.Public | BindingFlags.Static);
                _logErrorMethod = _worldType.GetMethod("LogError", BindingFlags.Public | BindingFlags.Static);
            }
        }

        public void Init()
        {
            _initMethod?.Invoke(null, Array.Empty<object>());
        }

        public void Run()
        {
            _runMethod?.Invoke(null, Array.Empty<object>());
        }

        public void LogInfo(string message)
        {
            _logInfoMethod?.Invoke(null, new object[] { message });
        }

        public void LogError(Exception exception)
        {
            _logErrorMethod?.Invoke(null, new object[] { exception });
        }
    }
}