using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._7.Models
{
    class Company
    {
        [Key]
        public int CompanyID { get; set; }
        public string Name { get; set; }

        public List<Phone> Phones { get; set; }

        public Company() { Phones = new List<Phone>(); }
        public Company(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();

            this.Name = Name; Phones = new List<Phone>();
        }
    }
}
