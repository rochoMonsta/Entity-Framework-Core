using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework5._10.Models
{
    class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string Name { get; set; }

        public Role() { }
        public Role(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException(nameof(Name));
            this.Name = Name;
        }
    }
}
