using System;
using System.Runtime.InteropServices;

namespace Trane.Submittals.Pipeline
{
  public enum PlatformType
  {
    Windows, Windows64,
    Mac, Mac64,
    Linux, Linux64
  }

  public enum EnvironmentType
  {
    Development,
    Production
  }

  public static class Reflection
  {
    private static readonly Lazy<PlatformType> _lazyplat = new Lazy<PlatformType>(GetPlatform);
    public static PlatformType GetPlatform()
    {
      if (_lazyplat.IsValueCreated)
        return _lazyplat.Value;
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        return Environment.Is64BitProcess ? PlatformType.Windows64 : PlatformType.Windows;
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        return Environment.Is64BitProcess ? PlatformType.Mac64 : PlatformType.Mac;
      else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        return Environment.Is64BitProcess ? PlatformType.Linux64 : PlatformType.Linux;
      else
        throw new NotSupportedException("Failure to recognize the current platform");
    }

    private static readonly Lazy<EnvironmentType> _lazyenv = new Lazy<EnvironmentType>(GetEnvironment);
    public static EnvironmentType GetEnvironment()
       => _lazyenv.IsValueCreated ? _lazyenv.Value
          : Enum.TryParse<EnvironmentType>(
              Environment.GetEnvironmentVariable("ENVIRONMENT")
           ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
           ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
              out var parsedenv) ? parsedenv
       : EnvironmentType.Development;

    public static bool IsEnvironment(string token, EnvironmentType type)
        => token != null
          ? string.Equals(token, type.ToString(),
             StringComparison.InvariantCultureIgnoreCase)
          : _lazyenv.Value == type;

    public static bool IsWindows()
       => _lazyplat.Value == PlatformType.Windows;
    public static bool IsWindows64()
       => _lazyplat.Value == PlatformType.Windows64;
    public static bool IsMac()
       => _lazyplat.Value == PlatformType.Mac;
    public static bool IsMac64()
       => _lazyplat.Value == PlatformType.Mac64;
    public static bool IsLinux()
       => _lazyplat.Value == PlatformType.Linux;
    public static bool IsLinux64()
       => _lazyplat.Value == PlatformType.Linux64;

    public static bool IsDevelopment(string token)
       => IsEnvironment(token, EnvironmentType.Development);
    public static bool IsProduction(string token)
       => IsEnvironment(token, EnvironmentType.Production);
  }
}
