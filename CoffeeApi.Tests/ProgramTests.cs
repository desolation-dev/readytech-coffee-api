using Microsoft.AspNetCore.Http.HttpResults;

namespace CoffeeApi.Tests;

public class ProgramTests
{
    [Fact]
    public void BrewCoffee_ReturnsJsonOfBrewCoffeeResponseResult()
    {
        // Act
        var actual = Program.BrewCoffee();

        // Assert
        Assert.IsType<JsonHttpResult<Program.BrewCoffeeResponse>>(actual);
    }
}
