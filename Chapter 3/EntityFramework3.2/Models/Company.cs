using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFramework3._2.Models
{
    class Company
    {
        [Key]
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public List<User> Users { get; set; }
    }
}
