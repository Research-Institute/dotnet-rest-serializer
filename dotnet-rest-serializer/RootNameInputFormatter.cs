using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace dotnet_rest_serializer
{
  public class RootNameInputFormatter : IInputFormatter
  {
    private Assembly _assembly;

    public RootNameInputFormatter(Assembly assembly)
    {
      _assembly = assembly;
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
        var responseJson = SerializationService.SerializeWithRoot(context.Object);

        await writer.WriteAsync(responseJson);

        await writer.FlushAsync();
      }
    }

    public bool CanRead(InputFormatterContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      var contentTypeString = context.HttpContext.Request.ContentType;

      return string.IsNullOrEmpty(contentTypeString) || contentTypeString == "application/json";
    }

    public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));
      var request = context.HttpContext.Request;

      if (request.ContentLength == 0)
      {
        return InputFormatterResult.SuccessAsync(null);
      }
      
      using (var reader = new StreamReader(context.HttpContext.Request.Body))
      {
        var model = SerializationService.DeserializeFromRoot(reader.ReadToEnd(), _assembly);
        return InputFormatterResult.SuccessAsync(model);
      }
    }
  }
}
