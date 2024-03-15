//using NuGet.Versioning;
//using System.Text.RegularExpressions;
//using System;

//namespace My.Utility;

//public partial class Semver
//{
//  const string AlphaReleaseLabel = "alpha";
//  const string BetaReleaseLabel = "beta";

//  public static Semver Default => From(1, 0, 0);

//  internal NuGetVersion Target { get; private set; }

//  private Semver(NuGetVersion semver) => Target = semver;

//  public int Major => Target.Major;
//  public int Minor => Target.Minor;
//  public int Patch => Target.Patch;
//  public string Release => Target.Release;

//  public bool IsPreRelease
//     => Target?.IsPrerelease == true;
//  public bool IsAlphaRelease
//     => string.Equals(Target?.Release, AlphaReleaseLabel,
//        StringComparison.InvariantCultureIgnoreCase);
//  public bool IsBetaRelease
//     => string.Equals(Target?.Release, BetaReleaseLabel,
//        StringComparison.InvariantCultureIgnoreCase);

//  public Semver NextMajor
//    => new Semver(new NuGetVersion(Major + 1, 0, 0, Release));
//  public Semver NextMinor
//    => new Semver(new NuGetVersion(Major, Minor + 1, 0, Release));
//  public Semver NextPatch
//    => new Semver(new NuGetVersion(Major, Minor, Patch + 1, Release));
//  public Semver NextRelease => (Release?.ToLower()) switch
//  {
//    AlphaReleaseLabel => new Semver(new NuGetVersion(1, 0, 0, BetaReleaseLabel)),
//    BetaReleaseLabel => new Semver(new NuGetVersion(1, 0, 0)),
//    _ => new Semver(Target),
//  };

//  public override bool Equals(object? o)
//     => Target == ((Semver?)o)?.Target;
//  public override int GetHashCode()
//     => base.GetHashCode();
//  public static bool operator ==(Semver a, Semver b)
//    => a.Target == b.Target;
//  public static bool operator !=(Semver a, Semver b)
//    => a.Target != b.Target;
//  public static bool operator <(Semver a, Semver b)
//    => a.Target < b.Target;
//  public static bool operator <=(Semver a, Semver b)
//    => a.Target <= b.Target;
//  public static bool operator >(Semver a, Semver b)
//    => a.Target > b.Target;
//  public static bool operator >=(Semver a, Semver b)
//    => a.Target >= b.Target;

//  public string ToPrefix() => $"{Major}.{Minor}.{Patch}";
//  public string ToSuffix() => Release;
//  public override string ToString() => Target.ToFullString();

//  public static Semver From(int major, int minor, int patch)
//    => new Semver(new NuGetVersion(major, minor, patch));
//  public static Semver From(int major, int minor, int patch, string release)
//    => new Semver(new NuGetVersion(major, minor, patch, release));

//  public static Semver Parse(string label)
//  {
//    label = GetParseEx().Replace(label, "");
//    return new Semver(NuGetVersion.Parse(label));
//  }
//  [GeneratedRegex("^\\s*[vV]")]
//  private static partial Regex GetParseEx();
//}
