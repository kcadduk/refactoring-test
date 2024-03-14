using System;

namespace LegacyApp.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            AddUser(args);
        }

        public static void AddUser(string[] args)
        {
            // DO NOT CHANGE THIS FILE AT ALL
            
            var userService = new UserService();
            var addResult = userService.AddUser("John", "Dinesh", "John.Dinesh@gmail.com", new DateTime(1993, 1, 1), 4);
            Console.WriteLine("Adding John Dinesh was " + (addResult ? "successful" : "unsuccessful"));
        }
    }
}