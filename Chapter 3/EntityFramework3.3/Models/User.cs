using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._3.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Name { get; set; }

        public int? CompanyID { get; set; }
        public virtual Company Company { get; set; }

        public int? PositionID { get; set; }
        public virtual Position Position { get; set; }

        public User() { }
        public User(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public User(string Name, Company Company, Position Position) : this(Name)
        {
            if ((Company is null) || (Position is null))
                throw new ArgumentNullException();
            this.Company = Company; this.Position = Position;
        }
        public override string ToString()
        {
            return $"User {this.UserID}:\n\tName: {this.Name};\n\t" +
                                          $"Company: {this.Company.Name};\n\t" +
                                          $"Position: {this.Position.Name};\n";
        }
    }
}
