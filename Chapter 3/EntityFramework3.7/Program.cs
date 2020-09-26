using EntityFramework3._7.Context;
using EntityFramework3._7.Models;
using System;
using System.Linq;

namespace EntityFramework3._7
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User()
            {
                Login = "rochoMonsta",
                Password = "lovenotfound",
                UserProfile = new UserProfile() 
                {
                    Name = new Claim() { Name = "Name", Value = "Roman" },
                    Surname = new Claim() { Name = "Surname", Value = "Cholkan" },
                    Age = new Claim() { Name = "Age", Value = "20" }
                }
            };
            var user2 = new User()
            {
                Login = "Mia12072000",
                Password = "thesunofthesun",
                UserProfile = new UserProfile() 
                {
                    Name = new Claim() { Name = "Name", Value = "Mia" },
                    Surname = new Claim() { Name = "Surname", Value = "Sorokotyaha" },
                    Age = new Claim() { Name = "Age", Value = "20" }
                }
            };

            AddNewUser(user1, user2);
            PrintAllUsers();

            Console.ReadLine();
        }
        public static void AddNewUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any(u => u.Login == user.Login && u.Password == user.Password))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
        public static void AddNewUser(params User[] users)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in users)
                {
                    if (!db.Users.Any(u => u.Login == user.Login && u.Password == user.Password))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
            }
        }
        public static void PrintAllUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in db.Users)
                    Console.WriteLine($"User ID: {user.UserID}\n\t" +
                                      $"Login: {user.Login}\n\t" +
                                      $"Password: {user.Password}\n\t" +
                                      $"Full name: {user?.UserProfile?.Name?.Value + " " + user?.UserProfile?.Surname?.Value}\n\t" +
                                      $"Age: {user?.UserProfile?.Age?.Value}");
            }
        }
    }
}
