using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFramework3._6.Models
{
    class Course
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public List<StudentCourse> StudentCourses { get; set; }

        public int TeacherID { get; set; }
        public Teacher Teacher { get; set; }

        public Course()
        {
            StudentCourses = new List<StudentCourse>();
        }
        public Course(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();
            this.Name = Name; StudentCourses = new List<StudentCourse>();
        }
    }
}
