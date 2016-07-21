using System.Reflection;

namespace dotnet_rest_serializer
{
  public class OutputPayloadFormatOptions : PayloadFormatOptions
  {
    public void UseClassNames()
    {
      FormatterStrategy = FormatterStrategies.ClassName;
    }
  }
}
