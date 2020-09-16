using EntityFramework2._11.Context;
using EntityFramework2._11.Models;
using System;
using System.Linq;

namespace EntityFramework2._11
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User("Roman", "Cholkan", 19);
            AddUser(user1);
            PrintUsers();

            Console.ReadLine();
        }
        public static void AddUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any(u => u.Name == user.Name && u.Surname == user.Surname && u.Age == user.Age))
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
                    if (!db.Users.Any(u => u.Name == user.Name && u.Surname == user.Surname && u.Age == user.Age))
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
