using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._9.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }

        public User() { }
        public User(string Name, string Surname)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException();
            this.Name = Name; this.Surname = Surname;
        }
        public User(string Name, string Surname, int Age) : this(Name, Surname)
        {
            if (Age < 0 || Age > 100)
                throw new ArgumentException();
            this.Age = Age;
        }
        public override string ToString()
        {
            return $"Person ID {this?.UserID}\n\t" +
                   $"Name: {this?.Name}\n\t" +
                   $"Surname: {this?.Surname}\n\t" +
                   $"Age: {this?.Age}";
        }
    }
}
