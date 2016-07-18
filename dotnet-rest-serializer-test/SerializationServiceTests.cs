using System.Collections.Generic;
using Xunit;
using dotnet_rest_serializer;

namespace dotnet_rest_serializer_test
{
  public class SerializationServiceTEsts
  {
    [Fact]
    public void SerializeWithRoot_SerializesSingularObjectsByType()
    {
      // arrange
      var obj = new TestClass();
      var expected = SerializationService.SerializeJson(new {TestClass = obj});
      // act
      var result = SerializationService.SerializeWithRoot(obj);

      // assert
      Assert.Equal(expected, result);
    }

    [Fact]
    public void SerializeWithRoot_SerializesPluralObjectsByType()
    {
      // arrange
      var obj = new List<TestClass>();

      var expected = SerializationService.SerializeJson(new { TestClasses = obj });
      // act
      var result = SerializationService.SerializeWithRoot(obj);

      // assert
      Assert.Equal(expected, result);
    }
  }
}
