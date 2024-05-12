using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Setup the endpoints
app.MapGet("/brew-coffee", BrewCoffee);

// Run the application
app.Run();


// Public partial class to allow easy access to the public methods for unit tests
public static partial class Program
{
    // Implementaion for the brew-coffee endpoint
    public static IResult BrewCoffee() => Results.Json(new {
        message = "Your piping hot coffee is ready",
        // ISO-8601 formated dateTime
        prepared = DateTimeOffset.Now.ToString("o", CultureInfo.InvariantCulture)
    });
}
