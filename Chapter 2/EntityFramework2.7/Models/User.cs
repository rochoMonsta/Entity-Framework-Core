using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework2._7.Models
{
    class User
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)] //відключення автогенерації значення
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // автогенерація значення
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; }
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
            return $"Person id: {this.Id};\n\t" +
                   $"Full name: {this.FullName}\n\t" +
                   $"Name: {this.Name};\n\t" +
                   $"Surname: {this.Surname};\n\t" +
                   $"Age: {this.Age};\n\t" +
                   $"Added at: {this.CreatedAt}";
        }
    }
}
