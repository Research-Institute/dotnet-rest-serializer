# dotnet-rest-serializer
A serializer to make dotnet work with Ember RESTAdapater

[![Build status](https://ci.appveyor.com/api/projects/status/v3aq62npp6dgs6y0?svg=true)](https://ci.appveyor.com/project/jaredcnance/dotnet-rest-serializer)

## About
For more information including usage, see the [blog post](http://nance.io/dotnet/ember/2016/07/18/ember-dotnet-core.html)

## Installation
[Install-Package dotnet-rest-serializer](https://www.nuget.org/packages/dotnet-rest-serializer/)

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
