using System.ComponentModel.DataAnnotations;

namespace EntityFramework2._10.Models
{
    class Product
    {
        //[Key]
        public int ProductID { get; set; }
        [Required]
        public string Name { get; set; }
        public double Price { get; set; }
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public override string ToString()
        {
            return $"Product ID: {this.ProductID};\n\t" +
                   $"Name: {this.Name};\n\t" +
                   $"Price: {this.Price};\n\t" +
                   $"Company ID: {this.CompanyID};\n";
        }
    }
}
