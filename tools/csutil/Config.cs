//using System.Collections.Generic;

//namespace My.Utility;

//public class Config
//{
//  private IConfigurationBuilder _builder;
//  private IConfigurationRoot? _root;

//  public IConfigurationRoot? Root => Validate();

//  public Config()
//     => _builder = new ConfigurationBuilder();

//  private void Invalidate()
//     => _root = null;

//  private IConfigurationRoot? Validate()
//     => _root ??= _builder.Build();

//  public Config SetPath(string path)
//  {
//    Invalidate();
//    _builder.SetBasePath(path);
//    return this;
//  }

//  public Config AddArgs(string[] args)
//  {
//    Invalidate();
//    _builder.AddCommandLine(args);
//    return this;
//  }

//  public Config AddEnvVars()
//  {
//    Invalidate();
//    _builder.AddEnvironmentVariables();
//    return this;
//  }

//  public Config AddFiles(params string[] paths)
//  {
//    Invalidate();
//    foreach (var path in paths)
//      _builder.AddJsonFile(path, optional: false);
//    return this;
//  }

//  public Config Merge(Config other)
//  {
//    Invalidate();
//    _builder.AddConfiguration(other.Validate());
//    return this;
//  }
//  public Config Merge(IDictionary<string, string?> dict)
//  {
//    Invalidate();
//    _builder.AddInMemoryCollection(dict);
//    return this;
//  }
//  public Config Merge(IEnumerable<KeyValuePair<string, string?>> pairs)
//  {
//    Invalidate();
//    _builder.AddInMemoryCollection(pairs);
//    return this;
//  }

//  public TValue? Get<TValue>(string key)
//    => Validate().GetValue<TValue>(key);

//  public TSettings? Bind<TSettings>()
//    => Validate().Get<TSettings>();

//  public TSettings? Bind<TSettings>(string section)
//    => Validate().GetValue<TSettings>(section);
//}
