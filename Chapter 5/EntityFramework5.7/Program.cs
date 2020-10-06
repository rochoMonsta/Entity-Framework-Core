using EntityFramework5._7.Context;
using EntityFramework5._7.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework5._7
{
    class Program
    {
        static void Main(string[] args)
        {
            var apple = new Company("Apple");
            var samsung = new Company("Samsung");
            var xiaomi = new Company("Xiaomi");

            var phone1 = new Phone("iPhone X", 799, apple);
            var phone2 = new Phone("iPhone 11", 849, apple);
            var phone3 = new Phone("Samsung Galaxy S9", 699, samsung);
            var phone4 = new Phone("Xiaomi Redmi 4X", 100, xiaomi);

            AddNewPhone(phone1, phone2, phone3, phone4);

            PrintAllPhonesWithGettingThem();
            Console.WriteLine("---------------");
            PrintAllPhonesWithoutGettingThem();

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
        public static void PrintAllPhonesWithoutGettingThem()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // Вказуємо, що для всього об'єкту контексту не потрібно виконувати відстеження
                // Всі елементи не поміщаються в кеш, як наслідок, будь які зміни не виконуються
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                
                foreach (var phone in db.Phones.Include(p => p.Company))
                    Console.WriteLine(phone);

                Console.WriteLine($"Count: {db.ChangeTracker.Entries().Count()}");
            }
        }
        public static void PrintAllPhonesWithGettingThem()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var phone in db.Phones.Include(p => p.Company))
                    Console.WriteLine(phone);

                // В даному випалку ми відстежуємо всі підтягнуті об'єкти, тому значення яке виведеться буде більше 0
                Console.WriteLine($"Count: {db.ChangeTracker.Entries().Count()}");
            }
        }
    }
}
