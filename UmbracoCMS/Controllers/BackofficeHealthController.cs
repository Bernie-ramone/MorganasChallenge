using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.Common.Routing;

namespace UmbracoCMS.Controllers
{
    [ApiController]
    [Route("api/health")] 
    public class BackofficeHealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult Check([FromQuery] bool isOk)
        {
            if (isOk)
                return Ok(new { message = "Everything is good!" });

            return BadRequest(new { message = "Something's wrong!" });
        }
    }
}
