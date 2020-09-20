using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework3._3.Models
{
    public class Position
    {
        [Key]
        public int PositionID { get; set; }
        public string Name { get; set; }

        public virtual List<User> Users { get; set; }

        public Position() { }
        public Position(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name;
        }
        public override string ToString()
        {
            return $"Position:\n\tName: {this.Name};\n";
        }
    }
}
