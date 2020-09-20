using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._3.Models
{
    public class Company
    {
        [Key]
        public int CompanyID { get; set; }
        public string Name { get; set; }

        public int CountryID { get; set; }
        public virtual Country Country { get; set; }
        public virtual List<User> Users { get; set; }

        public Company() { }
        public Company(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public Company(string Name, Country Country) : this(Name)
        {
            if (Country is null)
                throw new ArgumentNullException();
            this.Country = Country;
        }
        public override string ToString()
        {
            return $"Company:\n\tName: {this.Name};\n\tCountry: {this.Country.Name}\n";
        }
    }
}
