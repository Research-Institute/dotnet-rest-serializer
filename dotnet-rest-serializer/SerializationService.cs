using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Humanizer;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace dotnet_rest_serializer
{
  public static class SerializationService
  {
    public static string SerializeWithRoot(object entity, OutputPayloadFormatOptions outputPayloadFormatOptions)
    {
      // get the type name
      var typeName = GetTypeNameForSerialization(entity, outputPayloadFormatOptions);

      // create the response object
      var responseObject = new Dictionary<string, object> { [typeName] = entity };

      return SerializeJson(responseObject);
    }

    private static string GetTypeNameForSerialization(object entity, OutputPayloadFormatOptions outputPayloadFormatOptions)
    {
      switch (outputPayloadFormatOptions.GetFormatterStrategy())
      {
        case PayloadFormatOptions.FormatterStrategies.ClassName:
          // get the typeName from the class name
          return FormatTypeNameForDeserialization(entity);
        case PayloadFormatOptions.FormatterStrategies.ExplicitDefinition:
          // get the typeName from the SerializationDefinitions
          return outputPayloadFormatOptions.SerializationDefinitions.First(x => x.Key == entity.GetType()).Value;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// Formats a type name (singular or plural), used in the FormatterStrategies.ClassName
    /// </summary>
    /// <param name="entity">The entity to serialize</param>
    /// <returns>The formatted name</returns>
    private static string FormatTypeNameForDeserialization(object entity)
    {
      var type = entity.GetType();

      // get the type name of the object
      var typeName = type.Name;

      // if the object is a list, need to extract the list type and pluralize the name
      if (entity is IEnumerable)
        typeName = type.GetGenericArguments()[0].Name.Pluralize(inputIsKnownToBeSingular: false);

      return typeName;
    }

    public static string SerializeJson(object value)
    {
      var settings = new JsonSerializerSettings
      {
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new List<JsonConverter> { new StringEnumConverter() }
      };

      return JsonConvert.SerializeObject(value, settings);
    }
  }
}
