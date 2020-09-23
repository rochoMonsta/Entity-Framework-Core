using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFramework3._6.Models
{
    class Teacher
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

        public List<Course> Courses { get; set; }
    }
}
