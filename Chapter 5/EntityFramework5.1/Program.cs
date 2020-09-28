using EntityFramework5._1.Context;
using EntityFramework5._1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework5._1
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Create values
            var samsung = new Company("Samsung");
            var apple = new Company("Apple");
            var xiaomi = new Company("Xiaomi");

            var applePhone1 = new Phone() { Name = "iPhone X", Price = 899, Company = apple };
            var applePhone2 = new Phone() { Name = "iPhone 11", Price = 799, Company = apple };

            var samsungPhone1 = new Phone() { Name = "Samsung Galaxy Note 9", Price = 699, Company = samsung };
            var samsungPhone2 = new Phone() { Name = "Samsung Galaxy A41", Price = 399, Company = samsung };

            var xiaomiPhone1 = new Phone() { Name = "Xiaomi Redmi 4X", Price = 100, Company = xiaomi };
            var xiaomiPhone2 = new Phone() { Name = "Xiaomi Mi Max 2A", Price = 400, Company = xiaomi };
            #endregion

            #region Call methods
            AddNewPhone(
                applePhone1, applePhone2,
                samsungPhone1, samsungPhone2,
                xiaomiPhone1, xiaomiPhone2
                );
            PrintAllPhones();

            Console.WriteLine("\n");
            PrintPhonesByCompany(apple);
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
        public static void PrintAllPhones()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var phone in db.Phones.Include(p => p.Company))
                    Console.WriteLine(phone);
            }
        }
        public static void PrintPhonesByCompanyID(int companyID)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //var phoneCompaniesCollection = (from phone in db.Phones.Include(p => p.Company)
                //                                where phone.CompanyID == companyID
                //                                select phone).ToList();
                var phoneCompaniesCollection = db.Phones.Include(p => p.Company)
                                                        .Where(p => p.CompanyID == companyID);
                foreach (var phone in phoneCompaniesCollection)
                    Console.WriteLine(phone);
            }
        }
        public static void PrintPhonesByCompany(Company company)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //LinQ (Any) - вертає true якщо в колекції є хоча б один елемент який відповідає заданій умові
                if (db.Companies.Any(c => c.Name == company.Name))
                {
                    //LinQ (FirstOrDefault) - вертає об'єкт класу який задовільняє певній умові, якщо такого об'єкту немає, то вертає null
                    var companyID = db.Companies.FirstOrDefault(c => c.Name == company.Name).CompanyID;

                    //LinQ (Where) - відбирає ті елементи які задовільняють умові (вибірка з багатьох по умові)
                    var phonesByComapny = db.Phones.Include(p => p.Company)
                                                   .Where(p => p.CompanyID == companyID);
                    foreach (var phone in phonesByComapny)
                        Console.WriteLine(phone);
                }
            }
        }
        #endregion
    }
}
