using EntityFramework5._8.Context;
using EntityFramework5._8.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework5._8
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User() { Name = "Roman", Surname = "Cholkan", Age = 20 };

            AddNewUser(user1);
            ChangeUserWithTracking("Edward", "Balck", 19);
            Console.WriteLine();

            ChangeUserWithoutTracking("Edward", "Balck", 19);

            Console.ReadLine();
        }
        public static void AddNewUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any(
                    u => u.Name == user.Name && 
                    u.Surname == user.Surname &&
                    u.Age == user.Age))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
        public static void AddNewUser(params User[] users)
        {
            foreach (var user in users)
                AddNewUser(user);
        }
        public static void PrintUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                foreach (var user in db.Users)
                    Console.WriteLine(user);
            }
        }
        public static void ChangeUserWithTracking(string name, string surname, int age)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user1 = db.Users.FirstOrDefault();
                var user2 = db.Users.FirstOrDefault();

                Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" +
                                  $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}");

                user1.Name = name; user1.Surname = surname; user1.Age = age;

                Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" +
                                  $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}");
            }
        }
        public static void ChangeUserWithoutTracking(string name, string surname, int age)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user1 = db.Users.FirstOrDefault();
                var user2 = db.Users.AsNoTracking().FirstOrDefault();

                Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" +
                                  $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}");

                user1.Name = name; user1.Surname = surname; user1.Age = age;

                Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" +
                                  $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}");
            }
        }
    }
}
