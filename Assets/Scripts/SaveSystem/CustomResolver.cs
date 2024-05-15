using MessagePack;
using MessagePack.Formatters;
using MessagePack.Resolvers;
using Unity.VisualScripting;

public class CustomResolver : IFormatterResolver
{
  public static readonly IFormatterResolver Instance = new CustomResolver();
  private CustomResolver() { }
  public IMessagePackFormatter<T> GetFormatter<T>()
  {
    return Cache<T>.formatter;
  }

  private static class Cache<T>
  {
    public static readonly IMessagePackFormatter<T> formatter;

    static Cache()
    {
      if (typeof(T) == typeof(SaveData))
      {
        formatter = (IMessagePackFormatter<T>)new SaveDataFormatter();
      }
      else
      {
        formatter = StandardResolver.Instance.GetFormatter<T>();
      }
    }
  }
}
