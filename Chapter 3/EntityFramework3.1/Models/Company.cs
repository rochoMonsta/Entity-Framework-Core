using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._1.Models
{
    class Company
    {
        [Key]
        public int CompanyID { get; set; }
        [Required]
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
