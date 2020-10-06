using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._7.Models
{
    class Phone
    {
        [Key]
        public int PhoneID { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public Phone() { }
        public Phone(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public Phone(string Name, int Price) : this(Name)
        {
            if (Price < 0)
                throw new ArgumentException();
            this.Price = Price;
        }
        public Phone(string Name, int Price, Company Company) : this(Name, Price)
        {
            this.Company = Company;
        }
        public override string ToString()
        {
            return $"Phone ID: {this.PhoneID}\n\t" +
                   $"Name: {this?.Name}\n\t" +
                   $"Price: {this?.Price}\n\t" +
                   $"Company: {this?.Company?.Name}";
        }
    }
}
