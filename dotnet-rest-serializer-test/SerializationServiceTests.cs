using System.Collections.Generic;
using System.Reflection;
using Xunit;
using dotnet_rest_serializer.Services;
using dotnet_rest_serializer.Formatters;
using Humanizer.Inflections;

namespace dotnet_rest_serializer_test
{
  public class SerializationServiceTEsts
  {
    public SerializationServiceTEsts()
    {
      // if you run into issues with pluaralization, just add the definitions to the Humanizer Vocabulary
      Vocabularies.Default.AddSingular("Class", "Class");
      Vocabularies.Default.AddSingular("Classes", "Class");
    }

    [Fact]
    public void SerializeWithRoot_SerializesSingularObjectsByType()
    {
      // arrange
      var obj = new TestClass();
      var expected = SerializationService.SerializeJson(new {TestClass = obj});
      var options = new OutputPayloadFormatOptions();
      options.UseClassNames();

      // act
      var result = SerializationService.SerializeWithRoot(obj, options);

      // assert
      Assert.Equal(expected, result);
    }

    [Fact]
    public void SerializeWithRoot_SerializesPluralObjectsByType()
    {
      // arrange
      var obj = new List<TestClass>();
      var options = new OutputPayloadFormatOptions();
      options.UseClassNames();

      var expected = SerializationService.SerializeJson(new { TestClasses = obj });
      // act
      var result = SerializationService.SerializeWithRoot(obj, options);

      // assert
      Assert.Equal(expected, result);
    }

    [Fact]
    public void DeserializeFromRoot_DeSerializesSingularObjects()
    {
      // arrange
      var obj = new TestClass();
      var options = new InputPayloadFormatOptions();
      options.UseClassNames(Assembly.Load(new AssemblyName("dotnet-rest-serializer-test")));
      var json = SerializationService.SerializeJson(new { TestClass = obj });

      // act
      var result = DeSerializationService.DeserializeFromRoot(json, options);
      var typeResult = result as TestClass;

      // assert
      Assert.NotNull(typeResult);
    }

    [Fact]
    public void DeserializeFromRoot_DeSerializesPluralObjects()
    {
      // arrange
      var obj = new TestClass();
      var options = new InputPayloadFormatOptions();
      options.UseClassNames(Assembly.Load(new AssemblyName("dotnet-rest-serializer-test")));

      var json = SerializationService.SerializeJson(new { TestClasses = new List<TestClass>() { obj } });

      // act
      var result = DeSerializationService.DeserializeFromRoot(json, options);
      var typeResult = result as List<TestClass>;

      // assert
      Assert.NotNull(typeResult);
    }
  }
}
