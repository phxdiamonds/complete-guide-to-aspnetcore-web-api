using Microsoft.AspNetCore.Mvc;

namespace my_books.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [Route("api/[controller]")]
  // [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("get-test-data")]
        public IActionResult Getv1()
        {
            return Ok("This is Test Controller with version 1.0");
        }

        [HttpGet("get-test-data"), MapToApiVersion("2.0")]
        public IActionResult Getv2()
        {
            return Ok("This is Test Controller with version 2.0");
        }

        [HttpGet("get-test-data"),MapToApiVersion("3.0")]
        public IActionResult Getv3()
        {
            return Ok("This is Test Controller with version 3.0");
        }
    }
}
