using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFramework3._6.Models
{
    class Student
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int YearOfStudy { get; set; }

        public List<StudentCourse> StudentCourses { get; set; }
        
        public Student()
        {
            StudentCourses = new List<StudentCourse>();
        }
        public Student(string Name, int YearOfStudy)
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentNullException();

            if (YearOfStudy <= 0 || YearOfStudy > 6)
                throw new ArgumentException();

            this.Name = Name; this.YearOfStudy = YearOfStudy;

            StudentCourses = new List<StudentCourse>();
        }
    }
}
