using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Trane.Submittals.Pipeline
{
  public static class Hash
  {
    public static string GetSha256(string content)
    {
      string sha256 = null;
      if (content != null)
      {
        using (SHA256 mySHA256 = SHA256.Create())
        {
          StringBuilder sb = new StringBuilder();
          foreach (byte b in mySHA256.ComputeHash(Encoding.UTF8.GetBytes(content)))
            sb.Append(b.ToString("x2"));
          sha256 = sb.ToString();
        }
      }
      return sha256;
    }

    public static string GetSha256(Stream content)
    {
      string sha256 = null;
      if (content != null)
      {
        using (SHA256 mySHA256 = SHA256.Create())
        {
          StringBuilder sb = new StringBuilder();
          foreach (byte b in mySHA256.ComputeHash(content))
            sb.Append(b.ToString("x2"));
          sha256 = sb.ToString();
        }
      }
      return sha256;
    }
  }
}
