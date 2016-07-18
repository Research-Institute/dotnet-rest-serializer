using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Humanizer;
using System.Reflection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

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
    
    public static string SerializeJson(this object value)
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
