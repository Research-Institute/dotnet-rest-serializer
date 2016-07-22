using dotnet_rest_serializer.Formatters;

namespace dotnet_rest_serializer_example.Models
{
  [SerializationFormat("testClass", "testClasses")]
  public class TestClass
  {
    public string Name { get; set; }
  }
}
