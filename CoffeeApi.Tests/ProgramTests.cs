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
        // Arange
        int calls = numCalls;

        // Act
        var result = Program.BrewCoffee(ref calls).Result as StatusCodeHttpResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(503, result.StatusCode);

        Assert.Equal(numCalls + 1, calls);
    }

    [Fact]
    public void BrewCoffee_ReturnsStatusCode418Result()
    {
        // Arange
        int calls = 0;

        // Act
        var result = Program.BrewCoffee(ref calls, true).Result as StatusCodeHttpResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(418, result.StatusCode);
    }

    [Theory]
    [InlineData(2024)]
    [InlineData(2015)]
    [InlineData(1998)]
    [InlineData(2025)]
    [InlineData(2045)]
    public void IsAprilFirst_ReturnsTrue(int year)
    {
        // Arange
        DateTime dateTime = new(year, 4, 1);

        // Act
        bool actual = Program.IsAprilFirst(dateTime);

        // Assert
        Assert.True(actual);

    }

    [Theory]
    [InlineData(2024, 4, 2)]
    [InlineData(2020, 3, 31)]
    [InlineData(1998, 8, 1)]
    [InlineData(2013, 4, 16)]
    [InlineData(2025, 12, 1)]
    public void IsAprilFirst_ReturnsFalse(int year, int month, int day)
    {
        // Arange
        DateTime dateTime = new(year, month, day);
        
        // Act
        bool actual = Program.IsAprilFirst(dateTime);

        // Assert
        Assert.False(actual);
    }

}
