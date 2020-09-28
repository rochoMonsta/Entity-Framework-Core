using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFramework5._1.Models
{
    class Company
    {
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
