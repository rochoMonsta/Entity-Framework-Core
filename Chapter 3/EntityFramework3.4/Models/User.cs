using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._4.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public UserProfile Profile { get; set; }

        public User() { }
        public User(string Login, string Password)
        {
            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
                throw new ArgumentNullException();
            this.Login = Login; this.Password = Password;
        }
        public User(string Login, string Password, UserProfile Profile) : this (Login, Password)
        {
            this.Profile = Profile;
        }
    }
}
