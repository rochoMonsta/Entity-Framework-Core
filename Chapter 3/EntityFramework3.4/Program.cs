using EntityFramework3._4.Context;
using EntityFramework3._4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework3._4
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1Profile = new UserProfile() { Name = "Roman Cholkan", Age = 20 };
            var user2Profile = new UserProfile() { Name = "Solomiya Sorokotyaha", Age = 20 };

            var user1 = new User() { Login = "login1", Password = "password1", Profile = user1Profile };
            var user2 = new User() { Login = "login2", Password = "password2", Profile = user2Profile };

            AddUser(user1, user2);
            PrintUser();

            //Console.WriteLine("-----------------------------");

            //ChangeUser("login1", "password1", "Roman Cholkan", 20);
            //PrintUser();

            //Console.WriteLine("-----------------------------");
            //DeleteUser("login1", "password1"); // Remove "Roman Cholkan" from db and delete userProfile
            //DeleteUserProfile("login2", "password2"); // Remove userProfile from user but don't delete it
            //PrintUser();

            Console.ReadLine();
        }
        public static void AddUser(params User[] users)
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
        public static void PrintUser()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in db.Users.Include(u => u.Profile))
                    Console.WriteLine($"{user?.Profile?.Name} - {user?.Profile?.Age} - " +
                                      $"{user?.Login} - {user?.Password}");
            }
        }
        public static void ChangeUser(string login, string password, string name, int age)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var userProfile = db.UserProfiles.FirstOrDefault(u => u.User.Login == login && u.User.Password == password);
                if (userProfile != null)
                {
                    if (string.IsNullOrWhiteSpace(name))
                        throw new ArgumentNullException();
                    if (age < 0 || age > 100)
                        throw new ArgumentException();

                    userProfile.Name = name; userProfile.Age = age;
                    db.SaveChanges();
                }
            }
        }
        public static void DeleteUser(string login, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (user != null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }
        }
        public static void DeleteUserProfile(string login, string password)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var userProfile = db.UserProfiles.FirstOrDefault(u => u.User.Login == login && u.User.Password == password);
                if (userProfile != null)
                {
                    db.UserProfiles.Remove(userProfile);
                    db.SaveChanges();
                }
            }
        }
    }
}
