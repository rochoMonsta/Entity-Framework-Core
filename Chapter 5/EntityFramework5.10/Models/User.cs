using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._10.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }

        public User() { }
        public User(string FirstName, string LastName, int Age)
        {
            if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
                throw new ArgumentNullException(nameof(FirstName) + " " + nameof(LastName));

            if (Age > 100 || Age < 0)
                throw new ArgumentException(nameof(Age));
            this.FirstName = FirstName; this.LastName = LastName; this.Age = Age;
        }
        public User(string FirstName, string LastName, int Age, Role Role) : this(FirstName, LastName, Age)
        {
            this.Role = Role;
        }
        public override string ToString()
        {
            return $"User ID: {this.UserID}\n\t" +
                   $"Name: {this.FirstName}\n\t" +
                   $"Surname: {this.LastName}\n\t" +
                   $"Age: {this.Age}\n\t" +
                   $"Role: {this?.Role?.Name}\n";
        }
    }
}
