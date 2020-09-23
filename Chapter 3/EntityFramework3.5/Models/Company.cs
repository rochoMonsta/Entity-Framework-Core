using System;
using System.Collections.Generic;

namespace EntityFramework3._5.Models
{
    class Company
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }

        public List<User> Users { get; set; }

        public Company() { }
        public Company(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
    }
}
