using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace dotnet_rest_serializer
{
  public class RootNameInputFormatter : IInputFormatter
  {
    private readonly InputPayloadFormatOptions _inputFormatterOptions = new InputPayloadFormatOptions();

    public RootNameInputFormatter(Action<InputPayloadFormatOptions> options)
    {
      options.Invoke(_inputFormatterOptions);
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

      var model = DeSerializationService.DeserializeFromRoot(GetRequestBody(context.HttpContext.Request.Body), _inputFormatterOptions);

      return InputFormatterResult.SuccessAsync(model);
    }

    private string GetRequestBody(Stream body)
    {
      using (var reader = new StreamReader(body))
      {
        return reader.ReadToEnd();
      }
    }
  }
}
