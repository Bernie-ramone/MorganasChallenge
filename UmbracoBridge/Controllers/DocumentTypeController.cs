using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using UmbracoBridge.Models;
using UmbracoBridge.Services;
using UmbracoBridge.Validators;

namespace UmbracoBridge.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentTypeController : ControllerBase
    {
        private readonly IUmbracoManagementService _service;
        private readonly IConfiguration _config;

        public DocumentTypeController(IUmbracoManagementService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }

        [HttpGet("healthcheck")]
        public async Task<IActionResult> Healthcheck()
        {

            try
            {
                var token = await _service.GetTokenAsync();

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var url = $"{_config["Umbraco:Host"]}/umbraco/management/api/v1/health-check-group";
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, error);
                }

                var json = await response.Content.ReadAsStringAsync();
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DocumentTypeCreateRequest request)
        {
            var (isValid, errors) = DocumentTypeValidator.Validate(request);
            if (!isValid)
                return BadRequest(errors);

            var id = await _service.CreateDocumentTypeAsync(request);
            return CreatedAtAction(nameof(Create), new { id }, new { id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _service.DeleteDocumentTypeAsync(id);
            return NoContent();
        }
    }
}
