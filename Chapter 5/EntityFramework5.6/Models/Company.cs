using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._6.Models
{
    class Company
    {
        [Key]
        public int CompanyID { get; set; }
        public string Name { get; set; }

        public List<Phone> Phones { get; set; }

        public Company() { }
        public Company(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public override string ToString()
        {
            return $"Company ID: {this.CompanyID}\n\t" +
                   $"Name: {this?.Name}";
        }
    }
}
