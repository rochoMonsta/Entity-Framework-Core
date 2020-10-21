using EntityFramework5._10.Context;
using EntityFramework5._10.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework5._10
{
    class Program
    {
        static void Main(string[] args)
        {
            var admin = new Role("Admin");
            var user = new Role("User");

            var person1 = new User("Roman", "Cholkan", 20, admin);
            var person2 = new User("Mia", "Sorokotyaha", 20, admin);
            var person3 = new User("Edward", "Black", 19, user);
            var person4 = new User("Jack", "Sparrow", 54, user);
            var person5 = new User("Harry", "Potter", 15, user);
            var person6 = new User("Darth", "Vader", 78, admin);

            //AddNewUser(
            //    person1, person2, person3,
            //    person4, person5, person6
            //    );
            PrintAllUser(2);

            Console.WriteLine($"----------\n" +
                              $"{FindUserWithMinAge(2)}" +
                              $"----------");

            Console.WriteLine($"----------\n" +
                              $"{FindUserWithMinAge(1)}" +
                              $"----------");

            Console.ReadLine();
        }
        public static void AddNewUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any(u => u.FirstName == user.FirstName && 
                                       u.LastName == user.LastName && 
                                       u.Age == user.Age &&
                                       u.RoleID == db.Roles.FirstOrDefault(r => r.Name == user.Role.Name).RoleID))
                {
                    if (db.Roles.Any(r => r.Name == user.Role.Name))
                    {
                        user.RoleID = db.Roles.FirstOrDefault(r => r.Name == user.Role.Name).RoleID;
                        user.Role = null;
                    }
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
        public static void PrintAllUser(int roleID)
        {
            using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
            {
                foreach (var user in db.Users.Include(u => u.Role))
                    Console.WriteLine(user);
            }
        }
        public static void PrintAllUser(int roleID, bool ignoreFilters)
        {
            using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
            {
                if (ignoreFilters)
                {
                    foreach (var user in db.Users.IgnoreQueryFilters().Include(u => u.Role))
                        Console.WriteLine(user);
                }
                else
                    PrintAllUser(roleID);
            }
        }
        public static User FindUserWithMinAge(int roleID)
        {
            using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
            {
                var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Age == db.Users.Min(u => u.Age));

                return user == null ? null : user;
            }
        }
        public static User FindUserWithMinAge(int roleID, bool ignoreFilters)
        {
            using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
            {
                if (ignoreFilters)
                {
                    var user = db.Users.Include(u => u.Role).IgnoreQueryFilters().FirstOrDefault(u => u.Age == db.Users.Min(u => u.Age));

                    return user == null ? null : user;
                }
                else
                    return FindUserWithMinAge(roleID);
            }
        }
    }
}
