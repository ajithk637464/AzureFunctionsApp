using Microsoft.AspNetCore.Mvc;

namespace AzureFunctionWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FunctionCallerController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public FunctionCallerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("AddNumbers")]
        public async Task<IActionResult> AddNumbers(double a, double b)
        {
            // local function URL
            string functionUrl = $"http://localhost:7259/api/AddNumbersFunction?a={a}&b={b}";

            var response = await _httpClient.GetAsync(functionUrl);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Function call failed.");

            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }
        [HttpGet("get-weather")]
        public async Task<IActionResult> GetWeather([FromQuery] string city)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:7259/api/weather?city={city}");
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }
    }
}
