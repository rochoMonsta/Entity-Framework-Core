using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._4.Models
{
    class UserProfile
    {
        [Key]
        public int UserProfileID { get; set; }

        public string Name { get; set; }
        public int Age { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public UserProfile() { }
        public UserProfile(string Name, int Age)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            if (Age < 0 || Age > 100)
                throw new ArgumentException();

            this.Name = Name; this.Age = Age;
        }
        public UserProfile(string Name, int Age, User User) : this(Name, Age)
        {
            this.User = User;
        }
    }
}
