using System.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using System.Text.Json;

namespace AzureFunctionsApp
{
    public static class AddNumbersFunction
    {
        [Function("AddNumbersFunction")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
        {

            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            if (double.TryParse(query["a"], out var a) && double.TryParse(query["b"], out var b))
            {
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync($"Result: {a + b}");
                return response;
            }

            var data = await JsonSerializer.DeserializeAsync<Dictionary<string, double>>(req.Body);
            if (data != null && data.ContainsKey("a") && data.ContainsKey("b"))
            {
                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync($"Result: {data["a"] + data["b"]}");
                return response;
            }

            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Please provide 'a' and 'b' values (query or body).");
            return badResponse;
        }
    }
}
