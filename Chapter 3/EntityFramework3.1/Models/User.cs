using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFramework3._1.Models
{
    [Table("People")]
    class User
    {
        [Key]
        public int UserID { get; set; }
        [Required]
        public string Name { get; set; }
        public int CompanyID { get; set; } // Зовнішній ключ
        [ForeignKey("CompanyID")] // Встановлюємо свойство в якості зовнішнього ключа
        public Company Company { get; set; } // Навігаційний ключ

        public User() { }
        public User(string Name, Company Company)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();

            if (Company is null)
                throw new NullReferenceException();

            this.Name = Name; this.Company = Company;
        }
        public override string ToString()
        {
            return $"Person ID: {this.UserID};\n\t" +
                   $"Name: {this.Name};\n\t" +
                   $"Company ID: {this.CompanyID}\n";
        }
    }
}
