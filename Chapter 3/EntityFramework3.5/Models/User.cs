using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._5.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }

        public int CompanyID { get; set; }
        [Required]
        public Company Company { get; set; }

        public User() { }
        public User(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public User(string Name, Company Company) : this(Name)
        {
            this.Company = Company;
        }
    }
}
