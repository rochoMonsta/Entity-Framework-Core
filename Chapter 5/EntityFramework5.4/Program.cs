using EntityFramework5._4.Context;
using EntityFramework5._4.Models;
using System;
using System.Linq;

namespace EntityFramework5._4
{
    class Program
    {
        static void Main(string[] args)
        {
            var ukraine = new Country() { Name = "Ukraine" };
            var poland = new Country() { Name = "Poland" };
            var china = new Country() { Name = "China" };

            var samsung = new Company("Samsung", poland);
            var apple = new Company("Apple", ukraine);
            var xiaomi = new Company("Xiaomi", china);

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
            //PrintFullInfo();
            GroupByCompany();

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
                    if (db.Countries.Any(c => c.Name == phone.Company.Country.Name))
                    {
                        phone.Company.CountryID = db.Countries.FirstOrDefault(c => c.Name == phone.Company.Country.Name).ID;
                        phone.Company.Country = null;
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
                var phones = db.Phones.Join(db.Companies,
                    p => p.PhoneID,
                    c => c.CompanyID,
                    (p, c) => new
                    {
                        Name = p.Name,
                        Company = c.Name,
                        Price = p.Price
                    });
                foreach (var phone in phones)
                    Console.WriteLine($"Name: {phone.Name}\n\t" +
                                      $"Price: {phone.Price}\n\t" +
                                      $"Company: {phone.Company}");
            }
        }
        public static void PrintFullInfo()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // Групуємо дані з таблиць телефони, компанії, країни за основними ключами які поєднують ці таблиці
                var phones = from phone in db.Phones // Основна таблиця
                             join company in db.Companies on phone.CompanyID equals company.CompanyID // свойство селектора з першої таблиці
                             join country in db.Countries on company.CountryID equals country.ID // свойство селектора з другої таблиці
                             select new // новий анонімний тип який буде групувати всі основні елементи з всіх згруповани таблиць
                             {
                                 Name = phone.Name,
                                 Price = phone.Price,
                                 Company = company.Name,
                                 Country = country.Name
                             };
                foreach (var phone in phones)
                    Console.WriteLine($"Name: {phone.Name}\n\t" +
                                      $"Company: {phone.Company}\n\t" +
                                      $"Company location: {phone.Country}\n\t" +
                                      $"Price: {phone.Price}");
            }
        }
        public static void GroupByCompany()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // Групуємо телефони за назвою компанії яка їх створила
                var group = from phone in db.Phones
                            group phone by phone.Company.Name into dict
                            select new
                            {
                                dict.Key, // Ключем в даному випадку виступає назва компінй
                                Count = dict.Count() // Count - кількість телефонів які створила певна компанія
                            };
                foreach (var company in group)
                    Console.WriteLine($"Company {company.Key} - Phones {company.Count}");

                // Same to first example
                //var companyPhonesCount = db.Phones.GroupBy(p => p.Company.Name)
                //                                  .Select(g => new
                //                                  {
                //                                      g.Key,
                //                                      Count = g.Count()
                //                                  });
            }
        }
    }
}
