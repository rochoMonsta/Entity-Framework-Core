using EntityFramework2._7.Context;
using EntityFramework2._7.Models;
using System;
using System.Linq;

namespace EntityFramework2._7
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User("Roman", "Cholkan", 20);
            var user2 = new User("Mia", "Sorokotyaha", 20);

            AddUsers(user1, user2);
            PrintUsers();

            Console.ReadKey();
        }
        public static void AddUsers(params User[] users)
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in users)
                {
                    Console.WriteLine($"User index before adding to db: {user.Id}"); // 0
                    if (!db.Users.ToList().Any(us => us.Name == user.Name &&
                                                     us.Surname == user.Surname &&
                                                     us.Age == user.Age))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                    Console.WriteLine($"User index after adding to db: {user.Id}"); // 1
                }
            }
        }
        public static void PrintUsers()
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in db.Users.ToList())
                    Console.WriteLine(user);
            }
        }
    }
}
