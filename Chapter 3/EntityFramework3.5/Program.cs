using EntityFramework3._5.Context;
using EntityFramework3._5.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework3._5
{
    class Program
    {
        static void Main(string[] args)
        {
            var microsoft = new Company() { Name = "Microsoft" };
            var apple = new Company() { Name = "Apple" };
            var samsung = new Company() { Name = "Samsung" };

            var user1 = new User() { Name = "Roman Cholkan", Company = apple };
            var user2 = new User() { Name = "Mia Sorokotyaha", Company = apple };
            var user3 = new User() { Name = "Vadim Yakovlew", Company = microsoft };
            var user4 = new User() { Name = "Stiven Black", Company = samsung };

            AddUser(user1, user2, user3, user4);
            PrintCompaniesWorker();

            Console.WriteLine("-------------------------");
            DeleteUser(user3);
            PrintCompaniesWorker();

            Console.WriteLine("-------------------------");
            DeleteCompany(apple);
            PrintCompaniesWorker();
            //PrintUsers();

            Console.ReadLine();
        }
        public static void AddUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Companies.Any(c => c.Name == user.Company.Name))
                {
                    user.CompanyID = db.Companies.First(c => c.Name == user.Company.Name).CompanyID;
                    user.Company = null;
                }
                if (!db.Users.Any(u => u.Name == user.Name))
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
                    AddUser(user);
            }
        }
        public static void PrintUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in db.Users.Include(u => u.Company))
                {
                    Console.WriteLine($"User ID: {user.UserID};\n\tUser name: {user.Name};\n\t" +
                                      $"Company: {user.Company.Name};\n");
                }
            }
        }
        public static void PrintCompaniesWorker()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var company in db.Companies.Include(c => c.Users))
                {
                    Console.WriteLine($"Company: {company.Name}; ID: {company.CompanyID};");

                    foreach (var user in company.Users)
                        Console.WriteLine($"\tUser ID: {user.UserID}; Name: {user.Name}; Company id: {user.CompanyID};");
                    Console.WriteLine();
                }
            }
        }
        public static void DeleteUser(User user)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Users.Any(u => u.Name == user.Name))
                {
                    var deletedUser = db.Users.First(u => u.Name == user.Name);
                    db.Users.Remove(deletedUser);
                    db.SaveChanges();
                }
            }
        }
        public static void DeleteCompany(Company company)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Companies.Any(c => c.Name == company.Name))
                {
                    var deletedCompany = db.Companies.First(c => c.Name == company.Name);
                    db.Companies.Remove(deletedCompany);
                    db.SaveChanges();
                }
            }
        }
    }
}
