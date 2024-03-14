using System;
using LegacyApp;
using Xunit;

public class TimeProviderTests
{
    [Fact]
    public void Now_Returns_Current_DateTime()
    {
        // Arrange
        var timeProvider = new TimeProvider();

        // Act
        DateTime currentTime = timeProvider.Now;

        // Assert
        Assert.Equal(DateTime.Now, currentTime, TimeSpan.FromSeconds(1)); // Using TimeSpan to allow for a 1-second difference
    }
}
