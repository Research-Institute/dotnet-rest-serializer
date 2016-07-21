using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Newtonsoft.Json;

namespace dotnet_rest_serializer
{
  public static class DeSerializationService
  {

    /// <summary>
    /// Deserializes input JSON to type defined by the InputFormatterOptions
    /// </summary>
    /// <param name="entityJson">JSON to deserialize</param>
    /// <param name="inputFormatterOptions">Options for Formatting</param>
    /// <returns></returns>
    public static object DeserializeFromRoot(string entityJson, InputPayloadFormatOptions inputFormatterOptions)
    {
      // deserialize content to dictionary: { typeName: {} }
      var objectDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(entityJson);

      // get the type name
      var typeName = objectDictionary.Keys.First();

      // reserialize the object 
      var entity = JsonConvert.SerializeObject(objectDictionary[typeName]);

      // get the actual type to deserialize to 
      var type = GetTypeForDeserialization(typeName, inputFormatterOptions);

      // deserialize
      return JsonConvert.DeserializeObject(entity, type);
    }

    private static Type GetTypeForDeserialization(string typeName, InputPayloadFormatOptions inputFormatterOptions)
    {
      // format the typeName, entity => Entity or Entities => Entities
      return GetTypeBasedOnFormatterStrategy(inputFormatterOptions, typeName);
    }

    private static Type GetTypeBasedOnFormatterStrategy(InputPayloadFormatOptions inputFormatterOptions, string typeName)
    {
      switch (inputFormatterOptions.GetFormatterStrategy())
      {
        case PayloadFormatOptions.FormatterStrategies.ClassName:
          return GetTypeFromAssemblyClassName(inputFormatterOptions, typeName);
        case PayloadFormatOptions.FormatterStrategies.ExplicitDefinition:
          return GetTypeFromExplicitDefinition(inputFormatterOptions, typeName);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private static Type GetTypeFromAssemblyClassName(InputPayloadFormatOptions inputFormatterOptions, string typeName)
    {
      // typeName => TypeName
      typeName = typeName.Titleize().Replace(" ", "");
      // get the singular form of the entityName
      var singularTypeName = typeName.Singularize();
      // get the entity type from the assembly
      var entityType = inputFormatterOptions.Assembly.GetTypes().Single(t => t.Name == singularTypeName);
      // if the specified type name is singular, return the singular type, else return List<T>
      return singularTypeName == typeName ? entityType : typeof(List<>).MakeGenericType(entityType);
    }

    private static Type GetTypeFromExplicitDefinition(InputPayloadFormatOptions inputFormatterOptions, string typeName)
    {
      return inputFormatterOptions.SerializationDefinitions.Single(def => def.Value == typeName).Key;
    }
  }
}
