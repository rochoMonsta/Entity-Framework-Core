using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._3.Models
{
    public class Country
    {
        [Key]
        public int CountryID { get; set; }
        public string Name { get; set; }
        public int CapitalID { get; set; }
        public virtual City Capital { get; set; }
        public virtual List<Company> Companies { get; set; }

        public Country() { }
        public Country(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public Country(string Name, City Capital) : this(Name)
        {
            if (Capital is null)
                throw new ArgumentNullException();
            this.Capital = Capital;
        }
        public override string ToString()
        {
            return $"Country:\n\tName: {this.Name};\n\t" +
                   $"Capital: {this.Capital.Name};\n\t";
        }
    }
}
