using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Trane.Submittals.Pipeline
{
  public interface IFileRepo
  {
    Task<bool> Test(string path);
    Task Delete(string path);
    Task<Stream> StreamIn(string path);
    Task StreamOut(string path, Stream data);
    Task<string> TextIn(string path);
    Task TextOut(string path, string data);
    Task<bool> ZipEntryIn(string zipPath, string entryName, Stream entryData);
    Task ZipEntryOut(string zipPath, string entryName, string entryPath);
    Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, string>> entryPathMap);
    Task ZipEntryOut(string zipPath, string entryName, Stream entryData);
    Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, Stream>> entryDataMap);
  }

  public class LocalFileRepo : IFileRepo
  {
    public Task<bool> Test(string path)
    {
      return Task.FromResult(FsFile.Test(path));
    }

    public Task Delete(string path)
    {
      FsFile.Delete(path);
      return Task.CompletedTask;
    }

    public Task<Stream> StreamIn(string path)
    {
      return Task.FromResult((Stream)FsFile.StreamIn(path));
    }

    public Task StreamOut(string path, Stream data)
    {
      using (var ostream = FsFile.StreamOut(path))
        data.CopyTo(ostream);
      return Task.CompletedTask;
    }

    public async Task<string> TextIn(string path)
    {
      using (var fstream = new StreamReader(await StreamIn(path)))
        return fstream.ReadToEnd();
    }

    public Task TextOut(string path, string data)
    {
      using (var buffer = new MemoryStream(Encoding.UTF8.GetBytes(data)))
        return StreamOut(path, buffer);
    }

    public Task<bool> ZipEntryIn(string zipPath, string entryName, Stream entryData)
    {
      return Task.FromResult(FsZip.EntryIn(zipPath, entryName, entryData));
    }

    public Task ZipEntryOut(string zipPath, string entryName, string entryPath)
    {
      FsZip.EntryOut(zipPath, entryName, entryPath);
      return Task.CompletedTask;
    }

    public Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, string>> entryPathMap)
    {
      FsZip.EntryOut(zipPath, entryPathMap);
      return Task.CompletedTask;
    }

    public Task ZipEntryOut(string zipPath, string entryName, Stream entryData)
    {
      FsZip.EntryOut(zipPath, entryName, entryData);
      return Task.CompletedTask;
    }

    public Task ZipEntryOut(string zipPath, IEnumerable<KeyValuePair<string, Stream>> entryDataMap)
    {
      FsZip.EntryOut(zipPath, entryDataMap);
      return Task.CompletedTask;
    }
  }
}
