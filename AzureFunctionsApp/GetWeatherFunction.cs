using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Functions.Worker;

namespace AzureFunctionsApp
{
    public static class GetWeatherFunction
    {
        [Function("GetWeather")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "weather")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Processing GetWeather function request...");

            string city = req.Query["city"];

            if (string.IsNullOrEmpty(city))
            {
                using var reader = new StreamReader(req.Body);
                var body = await reader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(body);
                city = city ?? data?.city;
            }

            if (string.IsNullOrEmpty(city))
                return new BadRequestObjectResult("Please pass a city name in query or request body");

            // For sample purpose
            var result = new
            {
                City = city,
                Temperature = "29°C",
                Condition = "Partly Cloudy"
            };

            return new OkObjectResult(result);
        }
    }
}
