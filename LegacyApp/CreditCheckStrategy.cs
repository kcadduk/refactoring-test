using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyApp
{
    public interface ICreditCheckStrategy
    {
        void CheckCredit(User user);
    }

    public class VeryImportantClientCreditCheck : ICreditCheckStrategy
    {
        public void CheckCredit(User user)
        {
            user.HasCreditLimit = false;
        }
    }

    public class RegularClientCreditCheck : ICreditCheckStrategy
    {
        private readonly IUserCreditService _userCreditService;

        public RegularClientCreditCheck(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public void CheckCredit(User user)
        {
            user.HasCreditLimit = true;
            user.CreditLimit = _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
        }
    }

    public class ImportantClientCreditCheck : ICreditCheckStrategy
    {
        private readonly IUserCreditService _userCreditService;

        public ImportantClientCreditCheck(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public void CheckCredit(User user)
        {
            user.HasCreditLimit = true;
            user.CreditLimit = _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth) * 2;
        }
    }
}
