using EntityFramework2._6.Context;
using EntityFramework2._6.Models;
using System;
using System.Linq;

namespace EntityFramework2._6
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User("Roman", "Cholkan", 2213, 876987, 22222);
            var user2 = new User("Mia", "Sorokotyaha", 2213, 865423, 123321);


            AddUser(user1, user2);
            PrintUsers();

            Console.ReadLine();
        }
        public static void AddUser(User user)
        {
            using (UserContext db = new UserContext())
            {
                if (!db.Users.ToList().Any(pr => pr.Name == user.Name &&
                                                 pr.Surname == user.Surname &&
                                                 pr.PassportNumber == user.PassportNumber &&
                                                 pr.PassportSeria == user.PassportSeria &&
                                                 pr.PhoneNumber == user.PhoneNumber))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
        public static void AddUser(params User[] users)
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in users)
                {
                    if (!db.Users.ToList().Any(pr => pr.Name == user.Name &&
                                                 pr.Surname == user.Surname &&
                                                 pr.PassportNumber == user.PassportNumber &&
                                                 pr.PassportSeria == user.PassportSeria &&
                                                 pr.PhoneNumber == user.PhoneNumber))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
            }
        }
        public static void PrintUsers()
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in db.Users.ToArray())
                    Console.WriteLine(user);
            }
        }
    }
}
