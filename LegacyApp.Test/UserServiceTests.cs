using System;
using LegacyApp;
using Moq;
using Xunit;

namespace YourNamespace.Tests
{
    public class UserServiceTests
    {
        [Fact]
        public void AddUser_WithInvalidName_ReturnsFalse()
        {
            var mockClientRepository = new Mock<IClientRepository>();
            var mockUserCreditService = new Mock<IUserCreditService>();
            var mockUserDataAccessWrapper = new Mock<IUserDataAccessWrapper>();
            var mockTimeProvider = new Mock<ITimeProvider>();
            var mockCreditCheck = new Mock<ICreditCheckStrategyFactory>();

            var userService = new UserService(mockClientRepository.Object, mockUserCreditService.Object, mockUserDataAccessWrapper.Object, mockTimeProvider.Object, mockCreditCheck.Object);

            // Act
            var result = userService.AddUser("", "Dinesh", "dinesh@gmail.com", new DateTime(2000, 1, 1), 1);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void AddUser_WithValidData_CallsUserDataAccessWrapper()
        {
            var mockClientRepository = new Mock<IClientRepository>();
            var mockUserCreditService = new Mock<IUserCreditService>();
            var mockUserDataAccessWrapper = new Mock<IUserDataAccessWrapper>();
            var mockTimeProvider = new Mock<ITimeProvider>();
            var mockCreditCheck = new Mock<ICreditCheckStrategyFactory>();

            var userService = new UserService(mockClientRepository.Object, mockUserCreditService.Object, mockUserDataAccessWrapper.Object, mockTimeProvider.Object, mockCreditCheck.Object);

            var user = new User
            {
                Firstname = "Dinesh",
                Surname = "Babu",
                EmailAddress = "dinesh@gmail.com",
                DateOfBirth = new DateTime(1990, 1, 1),
                Client = new Client { Id = 1, Name = "RegularClient" }
            };

            mockClientRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns(user.Client);
            var result = userService.AddUser(user.Firstname, user.Surname, user.EmailAddress, user.DateOfBirth, user.Client.Id);
            Assert.True(result);
            mockUserDataAccessWrapper.Verify(wrapper => wrapper.AddUser(It.IsAny<User>()), Times.Once);
        }
    }
}
