using System;

namespace EFMigration
{
    class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public bool IsMarried { get; set; }
        public User() { }
        public User(string Name, string Surname, int Age)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name));

            if (string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException(nameof(Surname));

            if (Age < 0 || Age > 100)
                throw new ArgumentException(nameof(Age));

            this.Name = Name; this.Surname = Surname; this.Age = Age;
        }
        public override string ToString()
        {
            return $"ID: {this.Id};\nName: {this.Name};\nSurname: {this.Surname};\nAge: {this.Age};\nIs married: {this.IsMarried};\n";
        }
    }
}
