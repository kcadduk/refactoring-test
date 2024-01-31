using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;
        private readonly IUserRepository _userRepository;
        private readonly ITimeProvider _timeProvider;

        public UserService() : this(new ClientRepository(), new UserCreditServiceClient(), new UserRepository(), new TimeProvider())
        {
        }

        public UserService(IClientRepository clientRepository, IUserCreditService userCreditService, IUserRepository userRepository, ITimeProvider timeProvider)
        {
            _clientRepository = clientRepository ?? throw new ArgumentNullException(nameof(clientRepository));
            _userCreditService = userCreditService ?? throw new ArgumentNullException(nameof(userCreditService));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
        }

        public bool AddUser(string firstname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (FirstNameOrLastNameIsNullOrEmpty(firstname, surname))
            {
                return false;
            }

            if (!EmailIsValid(email))
            {
                return false;
            }

            int age = GetCurrentAge(dateOfBirth);
            if (age < 21)
            {
                return false;
            }

            var client = _clientRepository.GetById(clientId);
            var creditLimit = _userCreditService.GetCreditLimit(firstname, surname, dateOfBirth);
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firstname,
                Surname = surname,
                CreditLimit = creditLimit,
                HasCreditLimit = client.Name != ClientNameConstants.VeryImportantClient,
            };

            if (client.Name == ClientNameConstants.ImportantClient)
            {
                user.CreditLimit *= 2;
            }

            _userRepository.AddUser(user);

            return true;
        }

        private static bool FirstNameOrLastNameIsNullOrEmpty(string firstname, string surname)
        {
            return string.IsNullOrEmpty(firstname) || string.IsNullOrEmpty(surname);
        }

        private static bool EmailIsValid(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        private int GetCurrentAge(DateTime dateOfBirth)
        {
            var now = _timeProvider.Now;
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            return age;
        }
    }
}
