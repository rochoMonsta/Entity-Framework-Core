using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._3.Models
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string Name { get; set; }

        public City() { }
        public City(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public override string ToString()
        {
            return $"City:\n\tID: {this.CityID};\n\tName: {this.Name}";
        }
    }
}
