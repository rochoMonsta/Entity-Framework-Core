using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework2._4.Models
{
    class Person
    {
        private string name, surname;
        public int Id { get; set; }
        public string Name
        {
            get { return name; }
            set { if (value != null) { name = value; } }
        }
        public string Surname
        {
            get { return surname; }
            set { if (value != null) { surname = value; } }
        }
        public int Age { get; set; }
        [NotMapped]
        public Country Country { get; set; }
        public Person() { }
        public Person(string Name, string Surname, int Age)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException($"This {nameof(Name)} format is not correct.");
            if (string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException($"This {nameof(Surname)} format is not correct.");
            if (Age < 0 || Age > 100)
                throw new ArgumentException($"This {nameof(Age)} is not correct.");

            this.Name = Name; this.Surname = Surname; this.Age = Age;
        }
        public override string ToString()
        {
            return $"Person id: {this.Id}\n\tName: {this.Name};\n\t" +
                                           $"Surname: {this.Surname};\n\t" +
                                           $"Age: {this.Age};\n";
        }
    }
}
