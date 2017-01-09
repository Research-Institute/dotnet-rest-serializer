using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using dotnet_rest_serializer.Formatters;
using Humanizer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace dotnet_rest_serializer.Services
{
  public static class SerializationService
  {
    /// <summary>
    /// Serialize the object with the root. If no valid root can be determined, the object will be serialized as is.
    /// </summary>
    /// <param name="entity">The object to serialize</param>
    /// <param name="outputPayloadFormatOptions">Options for serialization</param>
    /// <returns>The entity as a serialized JSON string</returns>
    public static string SerializeWithRoot(object entity, OutputPayloadFormatOptions outputPayloadFormatOptions)
    {
      // get the type name
      if (entity == null) return string.Empty;
  
      var typeName = GetTypeNameForSerialization(entity, outputPayloadFormatOptions);
      
      var responseObject = string.IsNullOrEmpty(typeName) ? entity : new Dictionary<string, object> { [typeName] = entity };

      return SerializeJson(responseObject);
    }

    private static string GetTypeNameForSerialization(object entity, OutputPayloadFormatOptions outputPayloadFormatOptions)
    {
      switch (outputPayloadFormatOptions.GetFormatterStrategy())
      {
        case PayloadFormatOptions.FormatterStrategies.ClassName:
          // get the typeName from the class name
          return FormatTypeNameForDirectDeserialization(entity);
        case PayloadFormatOptions.FormatterStrategies.Dictionary:
          // get the typeName from the SerializationDefinitions
          return outputPayloadFormatOptions.SerializationDefinitions.FirstOrDefault(x => x.Key == entity.GetType()).Value;
        case PayloadFormatOptions.FormatterStrategies.Attribute:
          // get the typeName from the attributes on the class
          return FormatTypeNameForAttributeDeserialization(entity);
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// Formats a type name (singular or plural), used in the FormatterStrategies.ClassName
    /// </summary>
    /// <param name="entity">The entity to serialize</param>
    /// <returns>The formatted name</returns>
    private static string FormatTypeNameForDirectDeserialization(object entity)
    {
      var type = entity.GetType();

      // get the type name of the object
      var typeName = type.Name;

      // if the object is a list, need to extract the list type and pluralize the name
      if (entity is IEnumerable)
        typeName = type.GetGenericArguments()[0].Name.Pluralize(inputIsKnownToBeSingular: false);

      return typeName;
    }

    /// <summary>
    /// Formats a type name (singular or plural), used in the FormatterStrategies.ClassName
    /// </summary>
    /// <param name="entity">The entity to serialize</param>
    /// <returns>The formatted name</returns>
    private static string FormatTypeNameForAttributeDeserialization(object entity)
    {
      var type = entity.GetType();
      if (GetListOfPrimitiveLikeTypes().Contains(type))
        return string.Empty;

      if (entity is IEnumerable)
      {
		// gets the generic argument type such as List<T>
		var genericArguments = type.GetGenericArguments();

		if (genericArguments.Length == 0)
			return string.Empty;
		
		type = genericArguments[0];

        var attributes = TypeDescriptor.GetAttributes(type);
        return GetPluralNameFromAttributes(attributes);
      }
      else
      {
        var attributes = TypeDescriptor.GetAttributes(type);
        return GetSingularNameFromAttributes(attributes);
      }
    }

    private static string GetPluralNameFromAttributes(AttributeCollection attributes)
    {
      return ((SerializationFormat) attributes[typeof(SerializationFormat)])?.PluralName;
    }

    private static string GetSingularNameFromAttributes(AttributeCollection attributes)
    {
      return ((SerializationFormat) attributes[typeof(SerializationFormat)])?.SingluarName;
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

    private static List<Type> GetListOfPrimitiveLikeTypes()
    {
      return new List<Type>
      {
        typeof(decimal),
        typeof(string),
        typeof(bool),
        typeof(byte),
        typeof(sbyte),
        typeof(int),
        typeof(uint),
        typeof(char),
        typeof(double),
        typeof(float),
        typeof(long),
        typeof(DateTime),
        typeof(TimeSpan),
        typeof(DateTimeOffset),
        typeof(Guid),
        typeof(Enum)
      };
    }
  }
}
