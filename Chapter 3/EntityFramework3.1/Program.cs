using EntityFramework3._1.Context;
using EntityFramework3._1.Models;
using System;
using System.Linq;

namespace EntityFramework3._1
{
    class Program
    {
        static void Main(string[] args)
        {
            var apple = new Company("Apple");
            var microsoft = new Company("Microsoft");

            User[] users = {
                new User (Name: "Roman Cholkan", Company: apple),
                new User (Name: "Mia Sorokotyaha", Company: apple),
                new User (Name: "Orest Pavlow", Company: microsoft),
                new User (Name: "Yana Petros", Company: microsoft)
                };

            AddUser(users);
            PrintUsers();

            Console.ReadLine();
        }
        public static void AddUser(params User[] users)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in users)
                {
                    if (!db.Users.Any(u => u.Name == user.Name))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
            }
        }
        public static void PrintUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in db.Users)
                    Console.WriteLine(user);
            }
        }
    }
}
