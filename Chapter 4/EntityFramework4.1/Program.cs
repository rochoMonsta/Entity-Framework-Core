using EntityFramework4._1.Context;
using EntityFramework4._1.Models;
using System;
using System.Linq;

namespace EntityFramework4._1
{
    class Program
    {
        static void Main(string[] args)
        {
            User user1 = new User() { Name = "Roman", Surname = "Cholkan", Age = 20 };
            User user2 = new User() { Name = "Mia", Surname = "Sorokotyaha", Age = 20 };
            Employee employee1 = new Employee() { Name = "Robert", Surname = "Stone", Age = 45, Salary = 2000 };
            Employee employee2 = new Employee() { Name = "Emma", Surname = "Black", Age = 27, Salary = 3500 };
            Manager manager1 = new Manager() { Name = "Stiven", Surname = "Jobs", Age = 42, Salary = 9999, Departament = "IT" };

            AddUser(user1, user2, employee1, employee2, manager1);
            PrintUsers();

            Console.ReadLine();
        }
        public static void AddUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Users.Any(u => u.Name == user.Name && u.Surname == user.Surname))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
        public static void AddUser(params User[] users)
        {
            foreach (var user in users)
                AddUser(user);
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
