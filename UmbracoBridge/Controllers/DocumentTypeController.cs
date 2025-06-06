using Microsoft.AspNetCore.Mvc;
using UmbracoBridge.Models;
using UmbracoBridge.Validators;

namespace UmbracoBridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentTypeController : ControllerBase
    {
        [HttpGet("healthcheck")]
        public async Task<IActionResult> Healthcheck()
        {
            
            return Ok("Healthy");
        }

        [HttpPost]
        public IActionResult Create([FromBody] DocumentTypeCreateRequest request)
        {
            var (isValid, errors) = DocumentTypeValidator.Validate(request);
            if (!isValid)
                return BadRequest(errors);

            
            return Ok("Document Type created (fake response)");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            
            return Ok($"Deleted {id} (fake response)");
        }
    }
}
