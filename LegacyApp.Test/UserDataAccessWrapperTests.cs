using System;
using Xunit;
using Moq;
using LegacyApp;

public class UserDataAccessWrapperTests
{
    [Fact]
    public void AddUser_Calls_UserDataAccess_AddUser()
    {
        // Arrange
        var userDataAccessMock = new Mock<IUserDataAccessWrapper>();
        var user = new User
        {
            Firstname = "John",
            Surname = "Dinesh",
            EmailAddress = "dinesh@gmail.com",
            DateOfBirth = new DateTime(1990, 1, 1),
            Client = new Client { Id = 1, Name = "RegularClient" }
        };
        var userDataAccessWrapper = new UserDataAccessWrapper();

        // Act
        userDataAccessWrapper.AddUser(user);

        // Assert
        userDataAccessMock.Verify(u => u.AddUser(user), Times.Once);
    }
}
