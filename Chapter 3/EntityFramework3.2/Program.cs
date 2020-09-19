using EntityFramework3._2.Context;
using EntityFramework3._2.Models;
using System;
using System.Linq;

namespace EntityFramework3._2
{
    class Program
    {
        static void Main(string[] args)
        {
            var Apple = new Company() { Name = "Apple" };
            var Samsung = new Company() { Name = "Samsung" };

            User[] users =
            {
                new User("Vadim", "Yakovlew", 20, Samsung),
                new User("Roman", "Cholkan", 20, Apple),
                new User("Mia", "Sorokotyaha", 20, Apple),
                new User("Christina", "Malkush", 21, Samsung),
                new User("Obi-Van", "Kenobi", 36, Samsung),
                new User("Richard", "Black", 86, Apple)
            };

            AddUser(users);
            AddUser(new User("Emma", "Stone", 22, Samsung));
            PrintUser();

            Console.ReadLine();
        }
        public static void AddUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Companies.Any(c => c.Name == user.Company.Name))
                {
                    user.CompanyID = db.Companies.FirstOrDefault(c => c.Name == user.Company.Name).CompanyID;
                    user.Company = null;
                }
                if (!db.Users.Any(u => u.Name == user.Name &&
                                      u.Surname == user.Surname &&
                                      u.Age == user.Age))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
        public static void AddUser(params User[] users)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in users)
                {
                    if (db.Companies.Any(c => c.Name == user.Company.Name))
                    {
                        user.CompanyID = db.Companies.FirstOrDefault(c => c.Name == user.Company.Name).CompanyID;
                        user.Company = null;
                    }
                    if (!db.Users.Any(u => u.Name == user.Name &&
                                          u.Surname == user.Surname &&
                                          u.Age == user.Age))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
            }
        }
        public static void PrintUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in db.Users)
                    Console.WriteLine(user);
            }
        }
    }
}
