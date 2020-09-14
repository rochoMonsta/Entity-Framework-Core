using System;

namespace EntityFramework2._6.Models
{
    class User
    {
        //[Key] //Вказуємо, що свойство ProductNumber є ключем (id)
        public int UserNumber { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int PassportSeria { get; set; }
        public int PassportNumber { get; set; }
        public int PhoneNumber { get; set; }

        public User() { }
        public User(string Name, string Surname)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name));

            if (string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException(nameof(Surname));

            this.Name = Name; this.Surname = Surname;
        }
        public User(string Name, string Surname, int PassportSeria, int PassportNumber, int PhoneNumber) : this (Name, Surname)
        {
            if (PassportSeria < 0 || PassportNumber < 0 || PhoneNumber < 0)
                throw new ArgumentException();
            this.PassportNumber = PassportNumber; this.PassportSeria = PassportSeria; this.PhoneNumber = PhoneNumber;
        }
        public override string ToString()
        {
            return $"Person ID: {this.UserNumber}\n\t" +
                   $"Name: {this.Name};\n\t" +
                   $"Surname: {this.Surname};\n\t" +
                   $"Passport seria: {this.PassportSeria};\n\t" +
                   $"Passport number: {this.PassportNumber};\n\t" +
                   $"Phone number: {this.PhoneNumber};\n";
        }
    }
}
