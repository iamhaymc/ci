//using Stubble.Compilation;
//using Stubble.Compilation.Builders;
//using Stubble.Core.Interfaces;
//using System.IO;
//using System.Threading.Tasks;
//using System;

//namespace My.Utility;

//// https://mustache.github.io/mustache.5.html

//public class Template<T>
//{
//  public string Name { get; set; }
//  public Func<T, string> Render { get; set; }
//  public Template(string name, Func<T, string> render)
//  {
//    Name = name;
//    Render = render;
//  }
//}

//public class TemplateSettings
//{
//  public string TemplateDir { get; set; } = "Templates";
//  public string TemplateModelPrefix { get; set; } = "tmpl_";
//  public string TemplateSourcePrefix { get; set; } = "tmplsrc_";
//  public string TemplateSourceExtension { get; set; } = ".tmpl";
//  public bool EnableCache { get; set; } = true;
//}

//public interface ITemplateEngine
//{
//  string Invoke<TModel>(string name, TModel model);
//}

//public class TemplateEngine : ITemplateEngine
//{
//  private readonly TemplateSettings _config;
//  private readonly ILogger<TemplateEngine> _logger;
//  private readonly ICache _cache;
//  private readonly string _prefix;
//  private readonly StubbleCompilationRenderer _compiler;

//  public TemplateEngine(
//    IOptions<TemplateSettings> options,
//    ILogger<TemplateEngine> logger,
//    ICache cache)
//  {
//    _config = options.Value;
//    _logger = logger;
//    _cache = cache;
//    _prefix = GetType().AssemblyQualifiedName + ":";
//    _compiler = new StubbleCompilationBuilder().Configure(b =>
//    {
//      b.SetIgnoreCaseOnKeyLookup(false);
//      b.SetMaxRecursionDepth(512);
//      b.SetPartialTemplateLoader(
//        new TemplateReader(_config, _logger, _cache));
//    })
//    .Build();
//  }

//  public string Invoke<TModel>(string name, TModel model)
//  {
//    return Resolve<TModel>(name)?.Render(model);
//  }

//  public Template<TModel> Resolve<TModel>(string name)
//  {
//    Template<TModel> tmpl;
//    if (_config.EnableCache)
//    {
//      string key = _config.TemplateModelPrefix + name;
//      if (!_cache.TryGet(key, out tmpl))
//      {
//        tmpl = Compile<TModel>(name);
//        _cache.Set<Template<TModel>>(key, tmpl);
//      }
//    }
//    else tmpl = Compile<TModel>(name);
//    return tmpl;
//  }

//  public Template<TModel> Compile<TModel>(string name)
//  {
//    var path = Path.Join(_config.TemplateDir, name + _config.TemplateSourceExtension);
//    var source = File.ReadAllText(path);
//    return Compile<TModel>(name, source);
//  }

//  public Template<TModel> Compile<TModel>(string name, string source)
//  {
//    var render = _compiler.Compile<TModel>(source);
//    return new Template<TModel>(name, render);
//  }

//  public class TemplateReader : IStubbleLoader
//  {
//    private readonly TemplateSettings _config;
//    private readonly ILogger<TemplateEngine> _logger;
//    private readonly ICache _cache;

//    public TemplateReader(
//      TemplateSettings config,
//      ILogger<TemplateEngine> logger,
//      ICache cache)
//    {
//      _config = config;
//      _logger = logger;
//      _cache = cache;
//    }

//    public IStubbleLoader Clone()
//      => new TemplateReader(_config, _logger, _cache);

//    public string Load(string name)
//    {
//      string tsrc;
//      if (_config.EnableCache)
//      {
//        string key = _config.TemplateSourcePrefix + name;
//        if (!_cache.TryGet(key, out tsrc))
//        {
//          var path = Path.Join(_config.TemplateDir, name + _config.TemplateSourceExtension);
//          tsrc = File.ReadAllText(path);
//          _cache.Set<string>(key, tsrc);
//        }
//      }
//      else
//      {
//        var path = Path.Join(_config.TemplateDir, _config.TemplateSourceExtension);
//        tsrc = File.ReadAllText(path);
//      }
//      return tsrc;
//    }

//    public ValueTask<string> LoadAsync(string name)
//      => ValueTask.FromResult(Load(name));
//  }
//}

//public static class InjectorTemplateExtensions
//{
//  public static Injector AddTemplates(this Injector self, Config config)
//    => self.AddOptions<TemplateSettings>(config)
//      .Add<ITemplateEngine, TemplateEngine>();

//  public static ITemplateEngine? GetTemplateEngine(this Injector self)
//    => self.Get<ITemplateEngine>();
//}
