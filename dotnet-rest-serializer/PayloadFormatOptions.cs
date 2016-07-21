using System;
using System.Collections.Generic;

namespace dotnet_rest_serializer
{
  public class PayloadFormatOptions
  {
    protected FormatterStrategies FormatterStrategy = FormatterStrategies.ClassName;
    
    public Dictionary<Type, string> SerializationDefinitions { get; set; }

    public void UseExplicitDefinition(Dictionary<Type, string> serializationDefinitions)
    {
      FormatterStrategy = FormatterStrategies.ExplicitDefinition;
      SerializationDefinitions = serializationDefinitions;
    }
    
    public FormatterStrategies GetFormatterStrategy()
    {
      return FormatterStrategy;
    }

    public enum FormatterStrategies
    {
      ClassName,
      ExplicitDefinition
    }
  }
}
