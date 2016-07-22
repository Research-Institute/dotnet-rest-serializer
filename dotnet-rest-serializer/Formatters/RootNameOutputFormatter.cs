using System;
using System.Text;
using System.Threading.Tasks;
using dotnet_rest_serializer.Services;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace dotnet_rest_serializer.Formatters
{
  public class RootNameOutputFormatter : IOutputFormatter
  {
    private readonly OutputPayloadFormatOptions _outputPayloadFormatOptions = new OutputPayloadFormatOptions();

    public RootNameOutputFormatter(Action<OutputPayloadFormatOptions> options)
    {
      options.Invoke(_outputPayloadFormatOptions);
    }
    public bool CanWriteResult(OutputFormatterCanWriteContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      var contentTypeString = context.ContentType.ToString();

      return string.IsNullOrEmpty(contentTypeString) || contentTypeString == "application/json";
    }

    public async Task WriteAsync(OutputFormatterWriteContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      var response = context.HttpContext.Response;

      using (var writer = context.WriterFactory(response.Body, Encoding.UTF8))
      {
        var responseJson = SerializationService.SerializeWithRoot(context.Object, _outputPayloadFormatOptions);

        await writer.WriteAsync(responseJson);

        await writer.FlushAsync();
      }
    }
  }
}
