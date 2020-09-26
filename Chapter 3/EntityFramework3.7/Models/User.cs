using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._7.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
