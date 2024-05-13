using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CoffeeApi.Tests;

public class ProgramTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(20)]
    public void BrewCoffee_ReturnsJsonOfBrewCoffeeResponseResult(int numCalls)
    {
        // Arange
        int calls = numCalls;
        
        // Act
        var result = Program.BrewCoffee(ref calls).Result as JsonHttpResult<Program.BrewCoffeeResponse>;

        // Assert
        Assert.NotNull(result);
        
        Assert.Equal(200, result.StatusCode);

        Assert.NotNull(result.Value);
        Assert.IsType<Program.BrewCoffeeResponse>(result.Value);

        Assert.Equal(numCalls + 1, calls);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(9)]
    [InlineData(49)]
    public void BrewCoffee_ReturnsStatusCode503Result(int numCalls)
    {
        // Arrange
        int calls = numCalls;

        // Act
        var actual = Program.BrewCoffee(ref calls).Result as StatusCodeHttpResult;

        // Assert
        Assert.NotNull(actual);
        Assert.Equal(503, actual.StatusCode);

        Assert.Equal(numCalls + 1, calls);
    }
}
