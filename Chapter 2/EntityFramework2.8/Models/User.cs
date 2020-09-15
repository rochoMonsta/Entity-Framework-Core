using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework2._8.Models
{
    class User
    {
        #region Properties
        public int Id { get; set; }
        [Required] //NOT NULL
        [MaxLength(50)]
        public string Name { get; set; }
        [Required] //NOT NULL
        //[MaxLength(50)] 
        public string Surname { get; set; }
        [Required] //NOT NULL
        public int Age { get; set; }
        #endregion

        #region Constructors
        public User() { }
        public User(string Name, string Surname, int Age)
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
                throw new ArgumentNullException();
            if (Age < 0 || Age > 100)
                throw new ArgumentException();

            this.Name = Name; this.Surname = Surname; this.Age = Age;
        }
        #endregion

        #region Methods
        public override string ToString()
        {
            return $"Person ID: {this.Id}\n\t" +
                   $"Name: {this.Name};\n\t" +
                   $"Surname: {this.Surname};\n\t" +
                   $"Age: {this.Age};\n";
        }
        #endregion
    }
}
