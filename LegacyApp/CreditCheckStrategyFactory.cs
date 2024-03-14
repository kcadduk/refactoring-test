using System;

namespace LegacyApp
{
    public interface ICreditCheckStrategyFactory
    {
        ICreditCheckStrategy CreateCreditCheckStrategy(string clientType);
    }


    public class CreditCheckStrategyFactory : ICreditCheckStrategyFactory
    {
        private readonly IUserCreditService _userCreditService;

        public CreditCheckStrategyFactory(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public ICreditCheckStrategy CreateCreditCheckStrategy(string clientType)
        {
            switch (clientType)
            {
                case "VeryImportantClient":
                    return new VeryImportantClientCreditCheck();
                case "ImportantClient":
                    return new ImportantClientCreditCheck(_userCreditService);
                default:
                    return new RegularClientCreditCheck(_userCreditService);
            }
        }
    }

}
