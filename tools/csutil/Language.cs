using System.Threading;

namespace Trane.Submittals.Pipeline
{
  public class Language
  {
    // https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-lcid/a9eac961-e77d-41a6-90a5-ce1a8b0cdb9c

    /// <summary>
    /// Gets the LCID for English.
    /// </summary>
    public const int EnglishLanguageId = 9;

    /// <summary>
    /// Gets an LCID using the current culture
    /// </summary>
    public static int CurrentLanguageId => Thread.CurrentThread.CurrentCulture.LCID;
  }
}
