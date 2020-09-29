using EntityFramework5._3.Context;
using EntityFramework5._3.Models;
using System;
using System.Linq;

namespace EntityFramework5._3
{
    class Program
    {
        static void Main(string[] args)
        {
            var samsung = new Company("Samsung");
            var apple = new Company("Apple");
            var xiaomi = new Company("Xiaomi");

            var applePhone1 = new Phone() { Name = "iPhone X", Price = 899, Company = apple };
            var applePhone2 = new Phone() { Name = "iPhone 11", Price = 799, Company = apple };
            var applePhone3 = new Phone() { Name = "iPhone 6", Price = 500, Company = apple };
            var applePhone4 = new Phone() { Name = "iPhone 6S", Price = 599, Company = apple };
            var applePhone5 = new Phone() { Name = "iPhone 7", Price = 600, Company = apple };
            var applePhone6 = new Phone() { Name = "iPhone 7 Plus", Price = 750, Company = apple };

            var samsungPhone1 = new Phone() { Name = "Samsung Galaxy Note 9", Price = 699, Company = samsung };
            var samsungPhone2 = new Phone() { Name = "Samsung Galaxy A41", Price = 399, Company = samsung };

            var xiaomiPhone1 = new Phone() { Name = "Xiaomi Redmi 4X", Price = 100, Company = xiaomi };
            var xiaomiPhone2 = new Phone() { Name = "Xiaomi Mi Max 2A", Price = 400, Company = xiaomi };

            AddNewPhone(
                applePhone1, applePhone2, applePhone3, 
                applePhone4, applePhone5, applePhone6,
                samsungPhone1, samsungPhone2,
                xiaomiPhone1, xiaomiPhone2
                );
            //PrintAllPhones();

            //PhonesOrderByPrice();
            //PhonesOrderByDescendingPrice();
            PhonesOrderByName();

            Console.ReadLine();
        }
        #region Methods
        public static void AddNewPhone(Phone phone)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (!db.Phones.Any(p => p.Name == phone.Name))
                {
                    if (db.Companies.Any(c => c.Name == phone.Company.Name))
                    {
                        phone.CompanyID = db.Companies.FirstOrDefault(c => c.Name == phone.Company.Name).CompanyID;
                        phone.Company = null;
                    }
                    db.Phones.Add(phone);
                    db.SaveChanges();
                }
            }
        }
        public static void AddNewPhone(params Phone[] phones)
        {
            foreach (var phone in phones)
                AddNewPhone(phone);
        }
        public static void PrintAllPhones()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // Включаємо всі дані які є в Company

                //foreach (var phone in db.Phones.Include(p => p.Company))
                //    Console.WriteLine(phone);

                // Включаємо тільки Company.Name
                var phoneWithCompany = db.Phones.Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Company = p.Company.Name
                });
                foreach (var phone in phoneWithCompany)
                    Console.WriteLine($"Name: {phone.Name}\n\t" +
                                      $"Price: {phone.Price}\n\t" +
                                      $"Company: {phone.Company}\n");
            }
        }
        public static void PhonesOrderByPrice()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var sorted = db.Phones.Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    CompanyName = p.Company.Name
                }).OrderBy(p => p.Price).ThenBy(p => p.Name).ThenBy(p => p.CompanyName);

                foreach (var phone in sorted)
                    Console.WriteLine($"Name: {phone.Name}\n\t" +
                                      $"Price: {phone.Price}\n\t" +
                                      $"Company: {phone.CompanyName}\n");
            }
        }
        public static void PhonesOrderByDescendingPrice()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var sorted = db.Phones.Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    CompanyName = p.Company.Name
                }).OrderByDescending(p => p.Price).ThenBy(p => p.Name).ThenBy(p => p.CompanyName);

                foreach (var phone in sorted)
                    Console.WriteLine($"Name: {phone.Name}\n\t" +
                                      $"Price: {phone.Price}\n\t" +
                                      $"Company: {phone.CompanyName}\n");
            }
        }
        public static void PhonesOrderByName()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var sorted = db.Phones.Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Company = p.Company.Name
                }).OrderBy(p => p.Name).ThenBy(p => p.Company);

                foreach (var phone in sorted)
                    Console.WriteLine($"Name: {phone.Name}\n\t" +
                                      $"Price: {phone.Price}\n\t" +
                                      $"Company: {phone.Company}\n");
            }
        }
        public static void PhonesOrderByDescendingName()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var sorted = db.Phones.Select(p => new
                {
                    Name = p.Name,
                    Price = p.Price,
                    Company = p.Company.Name
                }).OrderByDescending(p => p.Name).ThenBy(p => p.Company);

                foreach (var phone in sorted)
                    Console.WriteLine($"Name: {phone.Name}\n\t" +
                                      $"Price: {phone.Price}\n\t" +
                                      $"Company: {phone.Company}\n");
            }
        }
        #endregion
    }
}
