using Microsoft.AspNetCore.Mvc;


namespace dotnet_rest_serializer_example.Controllers
{
  [Route("api/[controller]")]
  public class PrimitivesController : Controller
  {
    // GET api/primitives/string
    [HttpGet("string")]
    public IActionResult Get()
    {
      return Ok("string");
    }
  }
}
