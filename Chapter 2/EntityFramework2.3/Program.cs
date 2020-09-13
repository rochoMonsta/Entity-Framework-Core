using EntityFramework2._3.Context;
using System;

namespace EntityFramework2._3
{
    class Program
    {
        public static ProductsDBLogic productsLogic = new ProductsDBLogic();
        static void Main(string[] args)
        {
            Company Apple = new Company() { Name = "Apple" };
            Company Samsung = new Company() { Name = "Samsung" };

            Product product = new Product()
            {
                Name = "iPhone X",
                Price = 799,
                Company = Apple
            };
            Product product1 = new Product()
            {
                Name = "Samsung Galaxy S9",
                Price = 600,
                Company = Samsung
            };

            productsLogic.AddProduct(product);
            productsLogic.AddProduct(product1);

            productsLogic.PrintProducts();
            Console.ReadLine();
        }
    }
}
