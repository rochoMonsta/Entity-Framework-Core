using EntityFramework5._5.Context;
using EntityFramework5._5.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework5._5
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

            PrintPhoneWithCompany();

            Console.ReadLine();
        }
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
        public static void PrintPhoneWithCompany()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var phoneWithCompany = db.Phones.Where(p => p.Price > 700)
                                                .Union(db.Phones.Where(p => p.Name.Contains("Xiaomi")));

                foreach (var phone in phoneWithCompany)
                    Console.WriteLine(phone);

                ////var phones = db.Phones.Where(p => p.Price > 700)
                ////                      .Intersect(db.Phones.Where(p => p.Company.Name == "Apple"));

                ////Отримуємо колекцію тих телефонів, чия вартість перевищує 600
                //var selector1 = db.Phones.Where(p => p.Price > 600);
                ////Отримуємо колекцію тих телефонів, в який ім'я компанії - "Apple"
                //var selector2 = db.Phones.Where(p => p.Company.Name == "Apple");

                ////Отримуємо колекцію телефонів, які присутні в першій вибірці, але відсутні в другій
                ////Оскільки перша вибірка має багато телефонів компанії "Apple" та тільки 1 телефон компанії "Samsung"
                ////після виконання операції ми отримаємо тільки телефон компанї "Samsung", оскільки решту телефонів компанї "Apple"
                ////приустні в списку телефонів компанї "Apple"
                //var phones = selector1.Except(selector2);

                //foreach (var phone in phones.Include(p => p.Company))
                //    Console.WriteLine($"Phone name: {phone.Name} - Price: {phone.Price} - Company: {phone?.Company?.Name}");
            }
        }
    }
}
