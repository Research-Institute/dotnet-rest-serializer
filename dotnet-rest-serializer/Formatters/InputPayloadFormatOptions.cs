using System.Reflection;

namespace dotnet_rest_serializer.Formatters
{
  public class InputPayloadFormatOptions : PayloadFormatOptions
  {
    public Assembly Assembly { get; set; }
    public void UseClassNames(Assembly assembly)
    {
      FormatterStrategy = FormatterStrategies.ClassName;
      Assembly = assembly;
    }
  }
}
