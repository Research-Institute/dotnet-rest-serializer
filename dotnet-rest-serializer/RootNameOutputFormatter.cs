using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace dotnet_rest_serializer
{
  public class RootNameOutputFormatter : IOutputFormatter
  {
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
        var responseJson = SerializationService.SerializeWithRoot(context.Object);

        await writer.WriteAsync(responseJson);

        await writer.FlushAsync();
      }
    }
  }
}
