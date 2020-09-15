using EntityFramework2._8.Logic;
using EntityFramework2._8.Models;
using System;

namespace EntityFramework2._8
{
    class Program
    {
        private static UserDBLogic DBLogic = new UserDBLogic();
        static void Main(string[] args)
        {
            var user1 = new User("Roman", "Cholkan", 20);
            var user2 = new User("Mia", "Sorokotyaha", 20);

            //var user3 = new User(); // ERROR

            DBLogic.AddUser(user1, user2);
            DBLogic.PrintUsers();

            Console.ReadLine();
        }
    }
}
