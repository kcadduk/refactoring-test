namespace LegacyApp
{
    public class UserDataAccessWrapper : IUserDataAccessWrapper
    {
        public void AddUser(User user)
        {
            UserDataAccess.AddUser(user);
        }
    }
}