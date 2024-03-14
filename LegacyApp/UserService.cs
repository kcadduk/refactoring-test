using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;
        private readonly IUserDataAccessWrapper _userDataAccessWrapper;
        private readonly ITimeProvider _timeProvider;
        private readonly ICreditCheckStrategyFactory _creditCheckStrategyFactory;
        public UserService() : this(new ClientRepository(), new UserCreditServiceClient(), new UserDataAccessWrapper(), new TimeProvider(), new CreditCheckStrategyFactory(new UserCreditServiceClient()))
        {

        }
        public UserService(IClientRepository clientRepository, IUserCreditService userCreditService, IUserDataAccessWrapper userDataAccessWrapper, ITimeProvider timeProvider, ICreditCheckStrategyFactory creditCheckStrategyFactory)
        {
            _clientRepository = clientRepository;
            _userCreditService = userCreditService;
            _userDataAccessWrapper = userDataAccessWrapper;
            _timeProvider = timeProvider;
            _creditCheckStrategyFactory = creditCheckStrategyFactory;
        }
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (!Validate(firname, surname, email, dateOfBirth))
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);

            if (client == null)
                return false;

            var user = CreateUser(firname, surname, email, dateOfBirth, client);

            var creditCheckStrategy = _creditCheckStrategyFactory.CreateCreditCheckStrategy(client.Name);

            creditCheckStrategy.CheckCredit(user);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            _userDataAccessWrapper.AddUser(user);

            return true;
        }

        private int CalculateAge(DateTime dateOfBirth)
        {
            var now = _timeProvider.Now;
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            return age;
        }
        private User CreateUser(string firstName, string surname, string email, DateTime dateOfBirth, Client client)
        {
            return new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstName,
                Surname = surname
            };
        }
        private bool Validate(string firname, string surname, string email, DateTime dateOfBirth)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            if (CalculateAge(dateOfBirth) < 21)
            {
                return false;
            }
            return true;
        }
    }
}