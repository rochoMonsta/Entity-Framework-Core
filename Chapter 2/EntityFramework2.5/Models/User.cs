using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework2._5.Models
{
    [Table("People")] //Перевизначаємо співставлення моделі(User) з таблицею(People)
    class User
    {
        private string name, surname;
        [Column("user_id")] //Перевизначаємо співставлення свойства (Id) з назвою стовбця в таблиці (user_id)
        public int Id { get; set; }
        public string Name
        {
            get { return name; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    name = value;
            }
        }
        public string Surname
        {
            get { return surname; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    surname = value;
            }
        }
        public User() { }
        public User(string Name, string Surname)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException($"This type of {nameof(Name)} is not correct.");

            if (string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException($"This type of {nameof(Surname)} is not correct.");

            this.name = Name; this.surname = Surname;
        }
        public override string ToString()
        {
            return $"User ID: {this.Id};\n\tName: {this.Name};\n\tSurname: {this.Surname}\n";
        }
    }
}
