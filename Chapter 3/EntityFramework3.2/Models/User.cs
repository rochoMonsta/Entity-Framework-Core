using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._2.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }
        
        public User() { }
        public User(string Name, string Surname, int Age)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException();

            if (Age < 0 || Age > 100)
                throw new ArgumentException();

            this.Name = Name; this.Surname = Surname; this.Age = Age;
        }
        public User(string Name, string Surname, int Age, Company company) : this (Name, Surname, Age) { this.Company = company; }
        public override string ToString()
        {
            return $"User ID: {this.UserID};\n\t" +
                   $"User name: {this.Name};\n\t" +
                   $"User surname: {this.Surname};\n\t" +
                   $"User age: {this.Age};\n";
        }
    }
}
