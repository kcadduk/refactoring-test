using System;
using NSubstitute;
using Xunit;

namespace LegacyApp.UnitTests
{
    public class UserServiceTests
    {
        private readonly IClientRepository _clientRepository = Substitute.For<IClientRepository>();
        private readonly IUserCreditService _userCreditService = Substitute.For<IUserCreditService>();
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly ITimeProvider _timeProvider = Substitute.For<ITimeProvider>();
        private readonly UserService _sut;

        public UserServiceTests()
        {
            _clientRepository.GetById(0)
                .Returns(new Client
                {
                    Id = 0,
                    Name = ClientNameConstants.VeryImportantClient,
                    ClientStatus = ClientStatus.none
                });

            _timeProvider.Now.Returns(DateTime.Now);

            _sut = new UserService(_clientRepository, _userCreditService, _userRepository, _timeProvider);
        }

        [Fact]
        public void AddUser_WhenAllIsWell_ShouldReturnTrue()
        {
            var result = _sut.AddUser("doffi", "yoncs", "atest@email.com", new DateTime(1993, 12, 23), clientId: 0);

            Assert.True(result);
        }

        [Fact]
        public void AddUser_WhenFirstNameOrLastNameIsNullOrEmptry_ShouldReturnFalse()
        {
            var result = _sut.AddUser("doffi", "", "atest@email.com", new DateTime(1993, 12, 23), clientId: 0);

            Assert.False(result);
        }

        [Fact]
        public void AddUser_WhenEmailIsNotValid_ShouldReturnFalse()
        {
            var result = _sut.AddUser("doff", "yoncs", "atest@emailcom", new DateTime(1993, 12, 23), clientId: 0);

            Assert.False(result);
        }

        [Fact]
        public void AddUser_WheAgeIsLessThan21_ShouldReturnFalse()
        {
            var result = _sut.AddUser("doff", "yoncs", "atest@email.com", new DateTime(2010, 1, 1), clientId: 0);

            Assert.False(result);
        }
    }
}

