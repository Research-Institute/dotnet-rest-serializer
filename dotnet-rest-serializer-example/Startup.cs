﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using dotnet_rest_serializer;
using dotnet_rest_serializer_example.Models;

namespace dotnet_rest_serializer_example
{
  public class Startup
  {
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
          .AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      // Add framework services.
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
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      app.UseMvc();
    }
  }
}
