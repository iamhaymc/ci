using System;

namespace Trane.Submittals.Pipeline.Utility
{
  public static class Identifier
  {
    public static string Get() => Guid.NewGuid().ToString();

    //// Collision Calculator: https://zelark.github.io/nano-id-cc/
    //// With that alphabet (removed '_', '-', 'I'), length 21 and 1000 IDs per second:
    //// "~25 million years or 789,893T IDs needed, in order to have a 1% probability of at least one collision"
    //static string IdAlphabet = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    //public static string CreateId() => "I" + Nanoid.Nanoid.Generate(alphabet: IdAlphabet, size: 21);
  }
}
