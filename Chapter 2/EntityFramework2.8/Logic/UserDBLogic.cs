using EntityFramework2._8.Context;
using EntityFramework2._8.Models;
using System;
using System.Linq;

namespace EntityFramework2._8.Logic
{
    sealed class UserDBLogic
    {
        public void AddUser(User user)
        {
            using (UserContext db = new UserContext())
            {
                if (!db.Users.ToList().Any(person => person.Name == user.Name &&
                                                     person.Surname == user.Surname &&
                                                     person.Age == user.Age))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
        public void AddUser(params User[] users)
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in users)
                {
                    if (!db.Users.ToList().Any(person => person.Name == user.Name &&
                                                         person.Surname == user.Surname &&
                                                         person.Age == user.Age))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
            }
        }
        public void PrintUsers()
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in db.Users.ToArray())
                    Console.WriteLine(user);
            }
        }
    }
}
