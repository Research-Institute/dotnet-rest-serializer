using System;
using System.Collections.Generic;

namespace dotnet_rest_serializer.Formatters
{
  public class PayloadFormatOptions
  {
    protected FormatterStrategies FormatterStrategy = FormatterStrategies.ClassName;
    
    public Dictionary<Type, string> SerializationDefinitions { get; set; }

    /// <summary>
    /// Uses the provided dictionary to perform serialization.
    /// </summary>
    /// <param name="serializationDefinitions">Definitions for serialization and de-serialization.</param>
    public void UseDictionaryDefinition(Dictionary<Type, string> serializationDefinitions)
    {
      FormatterStrategy = FormatterStrategies.Dictionary;
      SerializationDefinitions = serializationDefinitions;
    }

    /// <summary>
    /// Uses the SerializationFormat attributes on classes to perform serialization.
    /// For deserialization, types must exist in the calling assembly
    /// </summary>
    public void UseAttributeDefinition()
    {
      FormatterStrategy = FormatterStrategies.Attribute;
    }

    public FormatterStrategies GetFormatterStrategy()
    {
      return FormatterStrategy;
    }

    public enum FormatterStrategies
    {
      ClassName,
      Dictionary,
      Attribute
    }
  }
}
