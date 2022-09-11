using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace api.Controllers;



[ApiController]
[Route("[controller]")]


public class TestController : Controller
{

    [HttpGet("tools/echo")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Generate a 200 with HttpGet")]
    public IActionResult echo1() => Ok("API Up. Versión 1.0");



    [HttpGet("tools/500")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Generate a 500 error with HttpGet")]
    public IActionResult Error500() => throw new Exception("500 Error of test");


    [HttpGet("tools/404")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Generate a 404 error with HttpGet")]
    public IActionResult Error404() => NotFound();

    [HttpGet("tools/401")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Generate a 401 error with HttpGet")]
    public IActionResult Error401() => Unauthorized();


    [HttpGet("tools/405")]
    [AllowAnonymous]
    [SwaggerOperation(Summary = "Generate a 405 error with HttpGet")]

    public IActionResult Error405()
    {
        Response.Headers.Add("Allow", "GET");
        return StatusCode(StatusCodes.Status405MethodNotAllowed);
    }
}
