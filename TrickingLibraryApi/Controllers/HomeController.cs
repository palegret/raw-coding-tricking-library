using Microsoft.AspNetCore.Mvc;

namespace TrickingLibraryApi.Controllers;

[ApiController]
[Route("api/home")]
public class Home : ControllerBase
{
    [HttpGet]
    public string Index()
    {
        return "Hello World";
    }
}
