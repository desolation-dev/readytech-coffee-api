using System.Globalization;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Http.HttpResults;

public class Program
{
    private const string OPEN_WEATHER_URL = "https://api.openweathermap.org/data/2.5/weather";
    private const string API_KEY = "a5d15c3dc316c9a6f5a802455899d143";
    private const string LAT = "-37.783333";
    private const string LON = "175.283333";
    private static string urlParameters = "?appid="+API_KEY+"&lat="+LAT+"&lon="+LON+"&units=metric";

    // SESSION DATA //
    /// <summary>
    /// The number of times the brew-coffee endpoint has been called
    /// </summary>
    static int brewCoffeeCalls = 0;

    public static HttpClient openWeatherHttpClient = new()
    {
        BaseAddress = new Uri(OPEN_WEATHER_URL)
    };

    // MAIN //
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // Setup the openApiHttpClient
        openWeatherHttpClient.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

        // Setup the endpoints
        app.MapGet("/brew-coffee", () => BrewCoffee(ref brewCoffeeCalls));

        // Run the application
        app.Run();
    }


    /// <summary>
    /// Implementation of the brew-coffee endpoint. Should return a 200 OK status code
    /// with a BrewCoffeeResponse in the respose body serialized as a JSON object. On 
    /// every fifth call, should return a status code 503 empty response instead.
    /// </summary>
    public static Results<JsonHttpResult<BrewCoffeeResponse>, StatusCodeHttpResult> BrewCoffee(ref int endpointCalls, bool aprilFirstTest=false) {
        // Check if April 1st. If so return joke status code 418
        if (IsAprilFirst(DateTime.Today) || aprilFirstTest) return TypedResults.StatusCode(418);
        // Update the amount of times this endpoint has been called
        endpointCalls++;
        // Return status code 503 if out of coffee (every fifth response)
        if (endpointCalls % 5 == 0)
        {
            return TypedResults.StatusCode(503);
        }
        // Return standard response
        else
        {
            // Default message
            string message = "Your piping hot coffee is ready";
            // Get the current temperature by querying the openWeather api
            HttpResponseMessage response = openWeatherHttpClient.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the returned json content
                var parsedResponse = JsonObject.Parse(response.Content.ReadAsStringAsync().Result);
                var jsonNode = parsedResponse?["main"]?["temp"];
                if (jsonNode != null)
                {
                    double temp = jsonNode.GetValue<double>();
                    if (temp > 30) message = "Your refreshing iced coffee is ready";
                }
                else
                {
                    Console.WriteLine("Failed to get current temperature");
                }
            }
            else
            {
                // Failed to get the current weather. Log error
                Console.WriteLine("Failed to get current weather");
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            // Finally return the brew coffee response
            return TypedResults.Json(
                new BrewCoffeeResponse {
                    Message = message,
                    // ISO-8601 formated dateTime
                    Prepared = DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture)
                }, statusCode: 200
            );
        }
    }


    /// <summary>
    /// The response object to return when calling the brew-coffee endpoint
    /// </summary>
    public class BrewCoffeeResponse
    {
        public required string Message { get; set; }
        public required string Prepared { get; set; }
    }

    /// <summary>
    /// Returns true if the passed DateTime object is April the 1st
    /// </summary>
    /// <param name="dateTime">The DateTime object to check</param>
    /// <returns>True if is April the 1st, False otherwise</returns>
    public static bool IsAprilFirst(DateTime dateTime)
    {
        return dateTime.Month == 4 && dateTime.Day == 1;
    }
}
