using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._8.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public User() { }
        public User(string Name, string Surname, int Age)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException();

            if (Age < 0 || Age > 100)
                throw new ArgumentException();

            this.Name = Name; this.Age = Age; this.Surname = Surname;
        }

        public override string ToString()
        {
            return $"User ID: {this.UserID}\n\t" +
                   $"Name: {this.Name}\n\t" +
                   $"Surname: {this.Surname}\n\t" +
                   $"Age: {this.Age}";
        }
    }
}
