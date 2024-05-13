using System.Globalization;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // Setup the endpoints
        app.MapGet("/brew-coffee", BrewCoffee);

        // Run the application
        app.Run();
    }

    /// <summary>
    /// Implementation of the brew-coffee endpoint. Should return a 200 OK status code
    /// with a BrewCoffeeResponse in the respose body serialized as a JSON object.
    /// </summary>
    public static IResult BrewCoffee () => Results.Json(
        new BrewCoffeeResponse {
            Message = "Your piping hot coffee is ready",
            // ISO-8601 formated dateTime
            Prepared = DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture)
        }
    );


    /// <summary>
    /// The response object to return when calling the brew-coffee endpoint
    /// </summary>
    public class BrewCoffeeResponse
    {
        public required string Message { get; set; }
        public required string Prepared { get; set; }
    }

}
