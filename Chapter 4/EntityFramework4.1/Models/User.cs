using System.ComponentModel.DataAnnotations;

namespace EntityFramework4._1.Models
{
    class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public override string ToString() => $"{this.UserID}) {this.Name} {this.Surname} {this.Age}";
    }
}
