using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework2._11.Models
{
    [Table("People")]
    class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public int Age { get; set; }

        public User() { }
        public User(string Name, string Surname, int Age)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException();

            if (Age < 0 || Age > 100)
                throw new ArgumentException();

            this.Name = Name; this.Surname = Surname; this.Age = Age;
        }

        public override string ToString()
        {
            return $"Person ID: {this.UserID}\n\t" +
                   $"Name: {this.Name};\n\t" +
                   $"Surname: {this.Surname};\n\t" +
                   $"Age: {this.Age};\n";
        }
    }
}
