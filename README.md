# dotnet-rest-serializer
A serializer to make dotnet work with Ember RESTAdapater

## Class Names

```
services.AddMvc(options =>
{
    options.InputFormatters.Insert(0, new RootNameInputFormatter(o =>
    {
        o.UseClassNames(myAssembly);
    }));
    options.OutputFormatters.Insert(0, new RootNameOutputFormatter(o =>
    {
        o.UseClassNames();
    }));
});
```

## Explicit Definitions

```
var serializationDefinitions = new Dictionary<Type, string> 
{ 
    { typeof(TestClass), "testClass" },
    { typeof(List<TestClass>), "testClasses" }
}; 
services.AddMvc(options => 
{ 
    options.InputFormatters.Insert(0, new RootNameInputFormatter(o =>
    {
        o.UseExplicitDefinition(serializationDefinitions);
    }));
    options.OutputFormatters.Insert(0, new RootNameOutputFormatter(o =>
    {
        o.UseExplicitDefinition(serializationDefinitions);
    }));
}); 
```

