using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._2.Models
{
    class Phone
    {
        [Key]
        public int PhoneID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public override string ToString()
        {
            return $"Phone ID: {this.PhoneID}\n\t" +
                   $"Name: {this.Name}\n\t" +
                   $"Price: {this.Price}\n\t" +
                   $"Company: {this?.Company?.Name}";
        }
    }
}
