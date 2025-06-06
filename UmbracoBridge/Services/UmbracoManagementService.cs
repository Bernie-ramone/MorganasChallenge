using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using UmbracoBridge.Models;

namespace UmbracoBridge.Services
{
    public interface IUmbracoManagementService
    {
        Task<string> CreateDocumentTypeAsync(DocumentTypeCreateRequest request);
        Task DeleteDocumentTypeAsync(string id);
        Task<string> GetTokenAsync();
    }
    public class UmbracoManagementService : IUmbracoManagementService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public UmbracoManagementService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> CreateDocumentTypeAsync(DocumentTypeCreateRequest request)
        {
            var token = await GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var body = JsonSerializer.Serialize(request);
            var content = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_config["Umbraco:Host"]}/umbraco/management/api/v1/document-type", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error creating Document Type: {error}");
            }

            if (response.StatusCode == HttpStatusCode.Created)
            {
                if (response.Headers.Location != null)
                {
                    var location = response.Headers.Location.ToString();
                    var id = location.Split('/').Last(); // Extrae el GUID del final
                    return id;
                }
                throw new Exception("Created but Location header not found.");
            }

            return string.Empty;

        }
        public async Task DeleteDocumentTypeAsync(string id)
        {
            var token = await GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"{_config["Umbraco:Host"]}/umbraco/management/api/v1/document-type/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error deleting Document Type: {error}");
            }
        }
        public async Task<string> GetTokenAsync()
        {
            var values = new Dictionary<string, string>
            {
                { "client_id", _config["Umbraco:ClientId"] },
                { "client_secret", _config["Umbraco:ClientSecret"] },
                { "grant_type", "client_credentials" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = await _httpClient.PostAsync($"{_config["Umbraco:Host"]}/umbraco/management/api/v1/security/back-office/token", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Token request failed: {error}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseBody))
                throw new Exception("Empty response from Umbraco when creating Document Type.");

            using var doc = JsonDocument.Parse(responseBody);
            return doc.RootElement.GetProperty("access_token").GetString()!;
        }
    }
}
