namespace dotnet_rest_serializer.Formatters
{
  public class OutputPayloadFormatOptions : PayloadFormatOptions
  {
    public void UseClassNames()
    {
      FormatterStrategy = FormatterStrategies.ClassName;
    }
  }
}
