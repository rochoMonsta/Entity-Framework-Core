using EntityFramework2._10.Context;
using EntityFramework2._10.Models;
using System;
using System.Linq;

namespace EntityFramework2._10
{
    class Program
    {
        static void Main(string[] args)
        {
            var Apple = new Company() { Name = "Apple" };
            var Samsung = new Company() { Name = "Samsung" };

            var phone1 = new Product()
            {
                Name = "iPhone X",
                Price = 899,
                Company = Apple
            };

            var phone2 = new Product()
            {
                Name = "Samsung Galaxy S9",
                Price = 699,
                Company = Samsung
            };

            var phone3 = new Product()
            {
                Name = "iPhone 11",
                Price = 999,
                Company = Apple
            };

            //AddProductToDB(phone1, phone2);
            AddProductToDB(phone3);
            PrintProducts();
            PrintCompanies();

            Console.ReadLine();
        }
        public static void AddProductToDB(Product product)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                //Перевіряємо чи в таблиці компаній є компанія виробник продукту
                //Якщо ні, то додаємо товар в таблицю товарів та компанію яка виготовила цей товар в таблицю компаній
                if (!db.Companies.ToList().Any(c => c.Name == product.Company.Name))
                    db.Products.Add(product);
                else
                {
                    //Якщо така компанія вже існує, отримуємо її індекс та присвоюємо його продукту як посилання на компанію
                    //Саме значення компанії для товару робимо null щоб компанії не дублювались
                    product.CompanyID = db.Companies.FirstOrDefault(c => c.Name == product.Company.Name).CompanyID;
                    product.Company = null;

                    if (!db.Products.ToList().Any(p => p.Name == product.Name &&
                                                       p.CompanyID == product.CompanyID))
                    {
                        db.Products.Add(product);
                    }
                }
                db.SaveChanges();
            }
        }
        public static void AddProductToDB(params Product[] products)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var product in products)
                {
                    //Перевіряємо чи в таблиці компаній є компанія виробник продукту
                    //Якщо ні, то додаємо товар в таблицю товарів та компанію яка виготовила цей товар в таблицю компаній
                    if (!db.Companies.ToList().Any(c => c.Name == product.Company.Name))
                        db.Products.Add(product);
                    else
                    {
                        //Якщо така компанія вже існує, отримуємо її індекс та присвоюємо його продукту як посилання на компанію
                        //Саме значення компанії для товару робимо null щоб компанії не дублювались
                        product.CompanyID = db.Companies.FirstOrDefault(c => c.Name == product.Company.Name).CompanyID;
                        product.Company = null;

                        if (!db.Products.ToList().Any(p => p.Name == product.Name &&
                                                           p.CompanyID == product.CompanyID))
                        {
                            db.Products.Add(product);
                        }
                    }
                    db.SaveChanges();
                }
            }
        }
        public static void PrintProducts()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var product in db.Products.ToArray())
                    Console.WriteLine(product);
            }
        }
        public static void PrintCompanies()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var company in db.Companies.ToArray())
                    Console.WriteLine(company);
            }
        }
    }
}
