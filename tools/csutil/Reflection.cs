using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Linq;
using System.Reflection;

namespace Trane.Submittals.Pipeline
{
  public class ObjectReflector
  {
    public object Target { get; private set; }
    public Type Type { get; private set; }

    public ObjectReflector(object target)
    {
      Target = target;
      Type = target.GetType();
    }

    #region [Type]

    public string Name => Type.Name;
    public string? FullName => Type.AssemblyQualifiedName;
    public Assembly Assembly => Type.Assembly;

    public bool Inherits<T>()
        => Type.IsAssignableFrom(typeof(T));

    #endregion

    #region [Properties]

    public PropertyInfo? GetProperty(string name)
        => Type.GetProperty(name);

    public IEnumerable<PropertyInfo> GetProperties()
        => Type.GetProperties();

    public IEnumerable<string> GetPropertyNames()
        => GetProperties().Select(m => m.Name);

    public T? GetPropertyValue<T>(string name)
        => (T?)GetPropertyValue(name);

    public object? GetPropertyValue(string name)
        => GetProperty(name)?.GetValue(name);

    public Dictionary<string, object?> GetPropertyValues()
        => GetProperties().ToDictionary(p => p.Name, p => p.GetValue(Target));

    public ObjectReflector SetPropertyValue(string name, object value)
    {
      GetProperty(name)?.SetValue(name, value);
      return this;
    }

    public ObjectReflector SetPropertyValue(Dictionary<string, object> values)
    {
      foreach (var entry in values)
        SetPropertyValue(entry.Key, entry.Value);
      return this;
    }

    #endregion

    #region [Methods]

    public MethodInfo? GetMethod(string name)
        => Type.GetMethod(name);

    public IEnumerable<MethodInfo> GetMethods()
        => Type.GetMethods();

    public IEnumerable<string> GetMethodNames()
        => GetMethods().Select(m => m.Name);

    public Delegate? GetMethodDelegate(string name)
        => GetMethod(name)?.ToDelegate(Target);

    public Dictionary<string, Delegate> GetMethodDelegates()
        => GetMethods().ToDictionary(p => p.Name, m => m.ToDelegate(Target));

    public object? InvokeMethod(string name, params object?[] args)
        => GetMethod(name)?.Invoke(Target, args);

    #endregion
  }

  public static class ObjectExtensions
  {
    public static ObjectReflector Reflect(this object self)
      => new ObjectReflector(self);
  }

  public static class AssemblyExtensions
  {
    public static DirectoryInfo GetDirectory(this Assembly self)
      => new DirectoryInfo(Path.GetDirectoryName(self.Location)!);
  }

  public static class MethodInfoExtensions
  {
    public static Delegate ToDelegate(this MethodInfo self, object target)
    {
      IEnumerable<Type> types;
      Func<Type[], Type> getType;
      bool isAction = self.ReturnType.Equals(typeof(void));
      if (isAction)
      {
        types = self.GetParameters()
            .Select(p => p.ParameterType);
        getType = Expression.GetActionType;
      }
      else
      {
        types = self.GetParameters()
            .Select(p => p.ParameterType).Concat(new[] { self.ReturnType });
        getType = Expression.GetFuncType;
      }
      return self.IsStatic
        ? Delegate.CreateDelegate(getType(types.ToArray()), self)
        : Delegate.CreateDelegate(getType(types.ToArray()), target, self.Name);
    }
  }
}
