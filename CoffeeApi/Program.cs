using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;

public class Program
{
    // SESSION DATA //
    /// <summary>
    /// The number of times the brew-coffee endpoint has been called
    /// </summary>
    static int brewCoffeeCalls = 0;


    // MAIN //
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

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
            return TypedResults.Json(
                new BrewCoffeeResponse {
                    Message = "Your piping hot coffee is ready",
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
