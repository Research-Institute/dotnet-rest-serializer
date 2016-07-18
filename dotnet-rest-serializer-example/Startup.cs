using System.Buffers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using dotnet_rest_serializer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
      services.AddMvc(options =>
      {
        options.OutputFormatters.Insert(0, new RootNameOutputFormatter());
      }).AddJsonOptions(opts =>
      {
        // Force Camel Case to JSON
        opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
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
