using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework2._10.Models
{
    class Company
    {
        public int CompanyID { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; }
        
        public Company()
        {
            Products = new List<Product>();
        }

        public override string ToString()
        {
            return $"Company ID: {this.CompanyID};\nName: {this.Name};\n";
        }
    }
}
