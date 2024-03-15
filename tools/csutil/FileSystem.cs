using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Trane.Submittals.Pipeline
{
  public static class FsPath
  {
    public static string Join(params string[] paths)
    {
      return Path.Combine(paths);
    }

    public static string[] Split(string path)
    {
      return path.Split(Path.DirectorySeparatorChar);
    }

    public static string Resolve(params string[] paths)
    {
      return Path.GetFullPath(Path.Combine(paths));
    }

    public static string Relative(string fromPath, string toPath)
    {
      // "Path.Relative" is not implemented in .NET Standard
      Uri fromUri = new Uri(fromPath);
      Uri toUri = new Uri(toPath);
      if (fromUri.Scheme != toUri.Scheme) return toPath;
      Uri relativeUri = fromUri.MakeRelativeUri(toUri);
      string relativePath = Uri.UnescapeDataString(relativeUri.ToString());
      if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
        relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
      return relativePath;
    }

    private static string Normalize(string path, bool isDirectory = false)
    {
      if (!string.IsNullOrEmpty(path))
      {
        path = path.Trim();
        if (isDirectory && path.Last() != Path.DirectorySeparatorChar)
          path += Path.DirectorySeparatorChar;
      }
      return path;
    }

    public static string NormalizeFile(string path)
      => Normalize(path, isDirectory: false);

    public static string NormalizeDir(string path)
      => Normalize(path, isDirectory: true);

    /// <summary>
    /// Gets the parent directory path
    /// </summary>
    public static string GetParent(string path)
    {
      return Path.GetDirectoryName(path);
    }

    /// <summary>
    /// Gets the file name with the extension
    /// </summary>
    public static string GetName(string path)
    {
      return Path.GetFileName(path);
    }

    /// <summary>
    /// Gets the file name without the extension
    /// </summary>
    public static string GetStem(string path)
    {
      return Path.GetFileNameWithoutExtension(path);
    }

    /// <summary>
    /// Gets the file extension
    /// </summary>
    public static string GetExtension(string path)
    {
      return Path.GetExtension(path);
    }

    public static string WithParent(string path, string parent)
    {
      return Path.Combine(parent, Path.GetFileName(path));
    }

    public static string WithName(string path, string name)
    {
      return Path.Combine(Path.GetDirectoryName(path), name);
    }

    public static string WithStem(string path, string stem)
    {
      return Path.Combine(Path.GetDirectoryName(path), stem + Path.GetExtension(path));
    }

    public static string WithExtension(string path, string suffix)
    {
      return Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path) + suffix);
    }

    public static string GetUniquePathStem(string prefix = "")
    {
      return prefix + Guid.NewGuid().ToString();
    }

    public static string GetUniqueFilePath(string stem, string extension, string parent = null, string prefix = "")
    {
      if (!extension.StartsWith(".")) extension = "." + extension;
      string path = $"{prefix}{stem}-{Guid.NewGuid()}{extension}";
      return !string.IsNullOrEmpty(parent) ? Path.Combine(parent, path) : path;
    }

    public static string GetUniqueTempDirPath(string prefix = "")
    {
      return Path.Combine(Path.GetTempPath(), GetUniquePathStem(prefix));
    }
  }

  public static class FsFile
  {
    private const int DEFAULT_BUFFER_SIZE = 1024;

    public static bool Test(string path)
    {
      return File.Exists(path);
    }

    public static void Move(string fromPath, string toPath)
    {
      File.Move(fromPath, toPath);
    }

    public static void Copy(string fromPath, string toPath)
    {
      Copy(new FileInfo(fromPath), new FileInfo(toPath));
    }

    public static void Copy(FileInfo fromPath, FileInfo toPath)
    {
      File.Copy(fromPath.FullName, toPath.FullName, overwrite: true);
    }

    /// <summary>
    /// Deletes the file if it exists
    /// </summary>
    public static void Delete(string path)
    {
      if (File.Exists(path))
        File.Delete(path);
    }

    public static long Measure(string path)
    {
      return new FileInfo(path).Length;
    }

    /// <summary>
    /// Computes the SHA 256 hash of the file
    /// </summary>
    public static string Hash(string path)
    {
      using (FileStream inputStream = StreamIn(path))
        return Pipeline.Hash.GetSha256(inputStream);
    }

    /// <summary>
    /// Creates the parent directory if it doesn't exist
    /// </summary>
    public static string Ensure(string path)
    {
      Directory.CreateDirectory(Path.GetDirectoryName(path));
      return path;
    }

    /// <summary>
    /// Returns an input file stream
    /// </summary>
    public static FileStream StreamIn(string path)
    {
      return File.OpenRead(path);
    }

    /// <summary>
    /// Returns an output file stream
    /// (The parent directory is ensured)
    /// </summary>
    public static FileStream StreamOut(string path)
    {
      return File.OpenWrite(Ensure(path));
    }

    /// <summary>
    /// Returns a binary file reader
    /// </summary>
    public static BinaryReader BinaryIn(string path)
    {
      return new BinaryReader(StreamIn(path), Encoding.Default, leaveOpen: false);
    }

    /// <summary>
    /// Returns a binary file writer
    /// (The parent directory is ensured)
    /// </summary>
    public static BinaryWriter BinaryOut(string path)
    {
      return new BinaryWriter(File.OpenWrite(Ensure(path)), Encoding.Default, leaveOpen: false);
    }

    /// <summary>
    /// Returns a text file reader
    /// </summary>
    public static StreamReader TextIn(string path)
    {
      return new StreamReader(StreamIn(path), Encoding.UTF8, detectEncodingFromByteOrderMarks: true, DEFAULT_BUFFER_SIZE, leaveOpen: false);
    }

    /// <summary>
    /// Returns a text file writer
    /// (The parent directory is ensured)
    /// </summary>
    public static StreamWriter TextOut(string path)
    {
      return new StreamWriter(File.OpenWrite(Ensure(path)), Encoding.UTF8, DEFAULT_BUFFER_SIZE, leaveOpen: false);
    }

    /// <summary>
    /// Reads all text from a file
    /// </summary>
    public static string StringIn(string path)
    {
      return File.ReadAllText(path);
    }

    /// <summary>
    /// Writes all text to file
    /// (The parent directory is ensured)
    /// </summary>
    public static void StringOut(string path, string data)
    {
      File.WriteAllText(Ensure(path), data);
    }

    /// <summary>
    /// Returns a specific model if JSON from a file
    /// </summary>
    public static T JsonIn<T>(string path)
    {
      JsonSerializerOptions options = Json.IndentByDefault ? Json.IndentedJsonOptions : Json.UnindentedJsonOptions;
      using (FileStream jsonStream = StreamIn(path))
        return JsonSerializer.Deserialize<T>(jsonStream, options);
    }

    /// <summary>
    /// Returns an generic model of JSON from a file
    /// </summary>
    public static JsonNode JsonIn(string path)
    {
      using (FileStream jsonStream = StreamIn(path))
        return JsonNode.Parse(jsonStream);
    }

    /// <summary>
    /// Writes JSON of a model to a file
    /// (The parent directory is ensured)
    /// </summary>
    public static void JsonOut<T>(string path, T data, bool indent = Json.IndentByDefault)
    {
      JsonSerializerOptions options = indent ? Json.IndentedJsonOptions : Json.UnindentedJsonOptions;
      using (FileStream jsonStream = StreamOut(Ensure(path)))
      using (Utf8JsonWriter jsonWriter = new Utf8JsonWriter(jsonStream))
        JsonSerializer.Serialize(jsonWriter, data, options);
    }
  }

  public static class FsDir
  {
    private const int MAX_CREATE_ATTEMPTS = 3;

    public static bool Test(string path)
    {
      return Directory.Exists(path);
    }

    public static void Ensure(string path)
    {
      Directory.CreateDirectory(path);
    }

    public static void Move(string fromPath, string toPath)
    {
      Directory.Move(fromPath, toPath);
    }

    public static void Copy(string fromPath, string toPath)
    {
      Copy(new DirectoryInfo(fromPath), new DirectoryInfo(toPath));
    }

    public static void Copy(DirectoryInfo fromPath, DirectoryInfo toPath)
    {
      Directory.CreateDirectory(toPath.FullName);
      foreach (FileInfo fi in fromPath.GetFiles())
        fi.CopyTo(Path.Combine(toPath.FullName, fi.Name), true);
      foreach (DirectoryInfo di in fromPath.GetDirectories())
        Copy(di, toPath.CreateSubdirectory(di.Name));
    }

    /// <summary>
    /// Deletes the directory if it exists
    /// </summary>
    public static void Delete(string path)
    {
      Directory.Delete(path);
    }

    public static DirectoryInfo CreateTemporary(string prefix = "")
    {
      int attempts = 0;
      string path = FsPath.GetUniqueTempDirPath(prefix);
      for (; attempts < MAX_CREATE_ATTEMPTS && !Test(path); ++attempts)
        path = FsPath.GetUniqueTempDirPath(prefix);
      if (attempts >= MAX_CREATE_ATTEMPTS)
        new Exception("Failure to create unique temporary directory");
      Directory.CreateDirectory(path);
      return new DirectoryInfo(path);
    }

    public static IEnumerable<FileInfo> FindFiles(string path, string namePattern = "*")
      => Directory.GetFiles(path, namePattern, SearchOption.TopDirectoryOnly).Select(x => new FileInfo(x));

    public static IEnumerable<FileInfo> FindAllFiles(string path, string namePattern = "*")
      => Directory.EnumerateFiles(path, namePattern, SearchOption.AllDirectories).Select(x => new FileInfo(x));

    public static IEnumerable<DirectoryInfo> FindDirs(string path, string namePattern = "*")
      => Directory.GetDirectories(path, namePattern, SearchOption.TopDirectoryOnly).Select(x => new DirectoryInfo(x));

    public static IEnumerable<DirectoryInfo> FindAllDirs(string path, string namePattern = "*")
      => Directory.EnumerateDirectories(path, namePattern, SearchOption.AllDirectories).Select(x => new DirectoryInfo(x));

    public static IEnumerable<FileSystemInfo> FindItems(string path, string namePattern = "*")
      => Directory.GetFileSystemEntries(path, namePattern, SearchOption.TopDirectoryOnly)
      .Select(x => File.Exists(x) ? (FileSystemInfo)new DirectoryInfo(x) : new FileInfo(x));

    public static IEnumerable<FileSystemInfo> FindAllItems(string path, string namePattern = "*")
      => Directory.GetFileSystemEntries(path, namePattern, SearchOption.AllDirectories)
      .Select(x => File.Exists(x) ? (FileSystemInfo)new DirectoryInfo(x) : new FileInfo(x));
  }

  public static class FsZip
  {
    public static bool Extract(string fromZipPath, string toDirPath)
    {
      if (FsFile.Test(fromZipPath))
      {
        new FastZip().ExtractZip(fromZipPath, toDirPath, null);
        return true;
      }
      return false;
    }

    public static bool EntryIn(string zipPath, string entryName, Stream entryData)
    {
      if (FsFile.Test(zipPath))
      {
        using (ZipFile zip = new ZipFile(zipPath))
        {
          zip.IsStreamOwner = true;
          ZipEntry entry = zip.GetEntry(entryName);
          if (entry != null)
          {
            using (Stream entryInput = zip.GetInputStream(entry))
              entryInput.CopyTo(entryData);
            return true;
          }
        }
      }
      return false;
    }

    public static void EntryOut(string zipPath, string entryName, string entryPath)
      => EntryOut(zipPath, new[] { new KeyValuePair<string, string>(entryName, entryPath) });

    public static void EntryOut(string zipPath, IEnumerable<KeyValuePair<string, string>> entryPathMap)
    {
      if (!FsFile.Test(zipPath))
      {
        using (FileStream zfs = File.Create(zipPath))
        {
          using (ZipOutputStream zip = new ZipOutputStream(zfs))
          {
            zip.IsStreamOwner = false;
            foreach (var entryArg in entryPathMap)
            {
              var newEntry = new ZipEntry(entryArg.Key);
              zip.PutNextEntry(newEntry);
              byte[] copyBuffer = new byte[4096];
              using (FileStream fs = File.OpenRead(entryArg.Value))
                StreamUtils.Copy(fs, zip, copyBuffer);
              zip.CloseEntry();
            }
          }
        }
      }
      else
      {
        using (ZipFile zip = new ZipFile(zipPath))
        {
          zip.IsStreamOwner = true;
          zip.BeginUpdate();
          foreach (var entryArg in entryPathMap)
          {
            using (var entryData = FsFile.StreamIn(entryArg.Value))
              zip.Add(new ZipEntryDataSource(entryData), entryArg.Key);
          }
          zip.CommitUpdate();
        }
      }
    }

    public static void EntryOut(string zipPath, string entryName, Stream entryData)
      => EntryOut(zipPath, new[] { new KeyValuePair<string, Stream>(entryName, entryData) });

    public static void EntryOut(string zipPath, IEnumerable<KeyValuePair<string, Stream>> entryDataMap)
    {
      if (!FsFile.Test(zipPath))
      {
        using (FileStream fs = File.Create(zipPath))
        {
          using (ZipOutputStream zip = new ZipOutputStream(fs))
          {
            zip.IsStreamOwner = false;
            foreach (var entryArg in entryDataMap)
            {
              var newEntry = new ZipEntry(entryArg.Key);

              zip.PutNextEntry(newEntry);
              byte[] copyBuffer = new byte[4096];
              StreamUtils.Copy(entryArg.Value, zip, copyBuffer);
              zip.CloseEntry();
            }
          }
        }
      }
      else
      {
        using (ZipFile zip = new ZipFile(zipPath))
        {
          zip.IsStreamOwner = true;
          zip.BeginUpdate();
          foreach (var entryArg in entryDataMap)
          {
            zip.Add(new ZipEntryDataSource(entryArg.Value), entryArg.Key);
          }
          zip.CommitUpdate();
        }
      }
    }

    public class ZipEntryDataSource : IStaticDataSource
    {
      public Stream Stream { get; set; }
      public ZipEntryDataSource(Stream stream) => Stream = stream;
      public Stream GetSource() => Stream;
    }
  }

  public class DisposableDirectory : IDisposable
  {
    public static DisposableDirectory Create(string prefix = "")
    {
      return new DisposableDirectory(FsDir.CreateTemporary(prefix));
    }

    private DirectoryInfo _info;
    public string Path => _info.FullName;
    public bool Exists => _info.Exists;

    public DisposableDirectory(DirectoryInfo info)
    {
      _info = info;
      Ensure();
    }

    public void Ensure()
    {
      if (!_info.Exists)
        _info.Create();
    }

    public void Delete()
    {
      if (_info.Exists)
        _info.Delete(recursive: true);
    }

    public void Dispose() => Delete();
  }

  public static class FileSystemInfoExtensions
  {
    public static bool IsDir(this FileSystemInfo self)
      => self.GetType() == typeof(DirectoryInfo);

    public static DirectoryInfo AsDir(this FileSystemInfo self)
      => (DirectoryInfo)self;

    public static DirectoryInfo? TryAsDir(this FileSystemInfo self)
      => self.IsDir() ? (DirectoryInfo)self : null;

    public static bool IsFile(this FileSystemInfo self)
      => self.GetType() == typeof(FileInfo);

    public static FileInfo AsFile(this FileSystemInfo self)
      => (FileInfo)self;

    public static FileInfo TryAsFile(this FileSystemInfo self)
      => self.IsFile() ? (FileInfo)self : null;

    public static DirectoryInfo JoinDir(this FileSystemInfo self, params string[] parts)
      => new DirectoryInfo(Path.Combine(new[] { self.FullName }.Concat(parts).ToArray()));

    public static FileInfo JoinFile(this FileSystemInfo self, params string[] parts)
      => new FileInfo(Path.Combine(new[] { self.FullName }.Concat(parts).ToArray()));
  }
}
