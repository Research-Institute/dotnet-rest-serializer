﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rest_serializer_example.Controllers
{
  [Route("api/[controller]")]
  public class ValuesController : Controller
  {
    // GET api/values
    [HttpGet]
    public IActionResult Get()
    {
      return Ok(new List<TestClass>() { new TestClass() });
    }

    // GET api/values/1
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
      return Ok(new TestClass());
    }
  }
}
