using EntityFramework2._5.Context;
using EntityFramework2._5.Models;
using System;
using System.Linq;

namespace EntityFramework2._5
{
    class Program
    {
        static void Main(string[] args)
        {
            var user1 = new User("Roman", "Cholkan");
            var user2 = new User("Mia", "Sorokotyaha");

            AddUser(user1, user2);
            PrintUsers();

            Console.ReadLine();
        }
        //public static void AddUser(User user)
        //{
        //    using (UserContext db = new UserContext())
        //    {
        //        if (!db.Users.ToList().Any(x => x.Name == user.Name && x.Surname == user.Surname))
        //        {
        //            db.Users.Add(user);
        //            db.SaveChanges();
        //        }
        //    }
        //}
        public static void AddUser(params User[] users)
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in users)
                {
                    if (!db.Users.ToList().Any(x => x.Name == user.Name && x.Surname == user.Surname))
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
