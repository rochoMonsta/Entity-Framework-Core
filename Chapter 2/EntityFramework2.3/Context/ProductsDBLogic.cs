using System;
using System.Linq;

namespace EntityFramework2._3.Context
{
    class ProductsDBLogic
    {
        public void AddProduct(Product product)
        {
            using (ProductContext db = new ProductContext())
            {
                if (!db.Products.ToList().Any(x => x.Name == product.Name))
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
            }
        }
        public void PrintProducts()
        {
            using (ProductContext db = new ProductContext())
            {
                foreach (var product in db.Products.ToList())
                    Console.WriteLine(product);
            }
        }
    }
}
