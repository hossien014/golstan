using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/department")]
public class CrudController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("ya mahdi");
    }
}