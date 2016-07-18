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
    public static string SerializeWithRoot(object entity)
    {
      // get the type name of the object
      var typeName = entity.GetType().Name;

      // if the object is a list, need to extract the list type and pluralize the name
      if (entity is IEnumerable)
        typeName = entity.GetType().GetGenericArguments()[0].Name.Pluralize(inputIsKnownToBeSingular: false);
      
      // create the response object
      var responseObject = new Dictionary<string, object> { [typeName] = entity };

      return SerializeJson(responseObject);
    }

    public static object DeserializeFromRoot(string entityJson, Assembly assembly)
    {
      var objectDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(entityJson);

      var typeName = objectDictionary.Keys.First();

      var entity = JsonConvert.SerializeObject(objectDictionary[typeName]);

      typeName = typeName.Titleize().Replace(" ", "");
      var singularTypeName = typeName.Singularize();

      var entityType = assembly.GetTypes().Single(t => t.Name == singularTypeName);

      if (singularTypeName == typeName)
      {
        return JsonConvert.DeserializeObject(entity, entityType);
      }
      else
      {
        var listType = typeof(List<>).MakeGenericType(entityType);
        return JsonConvert.DeserializeObject(entity, listType);
      }
      
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
