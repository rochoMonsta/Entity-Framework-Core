using EntityFramework5._6.Context;
using EntityFramework5._6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework5._6
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Create start data
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
            #endregion

            #region Call methods
            AddNewPhone(
                applePhone1, applePhone2, applePhone3,
                applePhone4, applePhone5, applePhone6,
                samsungPhone1, samsungPhone2,
                xiaomiPhone1, xiaomiPhone2
                );

            PrintPhonesForCompany("Samsung");

            Console.WriteLine($"DB has \"Samsung\": {CheckForCompany("Samsung")}");
            Console.WriteLine($"All phones created by \"Samsung\": {CheckAllPhonesForCompany("Samsung")}");
            Console.WriteLine($"Count \"Apple\" phones: {GetCountOfCompanyPhones("Apple")}");
            Console.WriteLine($"Total price for all \"Xiaomi\" phones: {GetTotalCompanyPhonePrice("Xiaomi")}");
            Console.WriteLine($"Phone with max price: {GetPhoneWithMaxPrice()}");
            Console.WriteLine($"Phone with min price: {GetPhoneWithMinPrice()}");
            Console.WriteLine($"Average price for \"Samsung\" phones: {AveragePriceForCompanyPhone("Samsung")}");

            #endregion
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
        public static bool CheckForCompany(string companyName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return false;
                else
                    return db.Companies.Any(c => c.Name == companyName);
            }
        }
        public static bool CheckAllPhonesForCompany(string companyName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return false;
                else
                    return db.Phones.All(p => p.Company.Name == companyName);
            }
        }
        public static int GetCountOfCompanyPhones(string companyName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return -1;
                else
                    return db.Phones.Count(p => p.Company.Name == companyName);
            }
        }
        public static int GetTotalCompanyPhonePrice(string companyName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return -1;
                else
                {
                    var phonesByCompany = db.Companies.Include(c => c.Phones)
                                                      .FirstOrDefault(c => c.Name == companyName).Phones;

                    if (phonesByCompany == null)
                        return -1;
                    else
                        return phonesByCompany.Sum(p => p.Price);
                }    
            }
        }
        public static Phone GetPhoneWithMaxPrice()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Phones.Include(p => p.Company).FirstOrDefault(p => p.Price == db.Phones.Max(p => p.Price));
            }
        }
        public static Phone GetPhoneWithMinPrice()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                return db.Phones.Include(p => p.Company).FirstOrDefault(p => p.Price == db.Phones.Min(p => p.Price));
            }
        }
        public static double AveragePriceForCompanyPhone(string companyName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    return -1;
                else
                    return db.Phones.Where(p => p.Company.Name == companyName)
                                    .Average(p => p.Price);
            }
        }

        public static void PrintPhonesForCompany(string companyName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (string.IsNullOrWhiteSpace(companyName))
                    throw new ArgumentNullException();
                else
                    foreach (var phone in db.Companies.Include(c => c.Phones).FirstOrDefault(c => c.Name == companyName).Phones)
                        Console.WriteLine(phone);
            }
        }
        #endregion
    }
}
