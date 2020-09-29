using EntityFramework5._2.Context;
using EntityFramework5._2.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework5._2
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
                applePhone1, applePhone2, applePhone3, applePhone4, applePhone5, applePhone6,
                samsungPhone1, samsungPhone2,
                xiaomiPhone1, xiaomiPhone2
                );

            //PrintCompanyPhones("Apple");
            //Console.WriteLine();

            //PrintAllPhonesByStartedWith("%Galaxy%");
            //Console.WriteLine();

            PrintAllPhonesByStartedWith("iPhone%[6-8]%");

            //Console.WriteLine(PhoneByID(5));

            Console.WriteLine(PhoneByIDFirstOrDefautl(5));

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
        public static void PrintPhonesByCompany(Company company)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Companies.Any(c => c.Name == company.Name))
                {
                    var companyID = db.Companies.FirstOrDefault(c => c.Name == company.Name).CompanyID;

                    var phonesByComapny = db.Phones.Include(p => p.Company)
                                                   .Where(p => p.CompanyID == companyID);
                    foreach (var phone in phonesByComapny)
                        Console.WriteLine(phone);
                }
            }
        }
        // LinQ Where
        // LinQ Any
        public static void PrintCompanyPhones(string companyName)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Companies.Any(c => c.Name == companyName))
                {
                    var collection = db.Phones.Include(p => p.Company).Where(p => p.Company.Name == companyName);

                    foreach (var phone in collection)
                        Console.WriteLine(phone);
                }
            }
        }
        /// <summary>
        /// Цей метод виводить ті телефони, в назві яких присутній /*name*/
        /// </summary>
        /// <param name="name" - елемент, який має бути присутнім в назві телефона для вибірки></param>
        public static void PrintAllPhonesByStartedWith(string name)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Phones.Any(p => EF.Functions.Like(p.Name, name)))
                {
                    var collections = db.Phones.Include(p => p.Company).Where(p => EF.Functions.Like(p.Name, name));

                    foreach (var phone in collections)
                        Console.WriteLine(phone);
                }
            } 
        }
        // Find - виконує пошук 1 елементу по заданому ID
        public static Phone PhoneByID(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                if (db.Phones.Any(p => p.PhoneID == id))
                    return db.Phones.Find(id);
                else
                    return null;
            }
        }
        public static Phone PhoneByIDFirstOrDefautl(int id)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var phone = db.Phones.Include(p => p.Company).FirstOrDefault(p => p.PhoneID == id);

                return phone == null ? null : phone;
            }
        }
        #endregion
    }
}
