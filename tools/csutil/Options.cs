//namespace Trane.Submittals.Pipeline
//{
//  public static class InjectorOptionsExtensions
//  {
//    public static Injector AddOptions<T>(this Injector self, Config config) where T : class
//       => self.AddOptions<T>(config.Root);

//    public static Injector AddOptions<T>(this Injector self, Config config, string section) where T : class
//       => self.AddOptions<T>(config.Root.GetSection(section));

//    public static Injector AddOptions<T>(this Injector self, IConfiguration section) where T : class
//       => self.Edit(c => c.Configure<T>(section));

//    public static IOptionsMonitor<T>? GetOptionsMonitor<T>(this Injector self) where T : class
//       => self.Get<IOptionsMonitor<T>>();

//    public static IOptions<T>? GetOptions<T>(this Injector self) where T : class
//       => self.Get<IOptions<T>>();
//  }
//}
