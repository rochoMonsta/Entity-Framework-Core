using EntityFramework3._6.Context;
using EntityFramework3._6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework3._6
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var teacher1 = new Teacher() { Name = "Robert Doms" };
                var teacher2 = new Teacher() { Name = "Obi-Van Kenobi" };
                db.Teachers.AddRange(teacher1, teacher2);

                var student1 = new Student() { Name = "Roman Cholkan", YearOfStudy = 4 };
                var student2 = new Student() { Name = "Mia Sorokotyaha", YearOfStudy = 3 };
                var student3 = new Student() { Name = "Vadim Yakovlew", YearOfStudy = 4 };
                var student4 = new Student() { Name = "Emma Stone", YearOfStudy = 5 };
                db.Students.AddRange(student1, student2, student3, student4);

                var math = new Course() { Name = "Math", Teacher = teacher2 };
                var history = new Course() { Name = "History", Teacher = teacher1};
                db.Courses.AddRange(math, history);

                db.SaveChanges();

                student1.StudentCourses.Add(new StudentCourse() { StudentID = student1.ID, CourseID = math.ID });
                student2.StudentCourses.Add(new StudentCourse() { StudentID = student2.ID, CourseID = math.ID });
                student3.StudentCourses.Add(new StudentCourse() { StudentID = student3.ID, CourseID = history.ID });
                student4.StudentCourses.Add(new StudentCourse() { StudentID = student4.ID, CourseID = history.ID });

                db.SaveChanges();

                var courses = db.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student).ToList();
                foreach (var course in courses)
                {
                    Console.WriteLine($"Couse: {course.Name}");
                    var students = course.StudentCourses.Select(sc => sc.Student).ToList();
                    foreach (var student in students)
                        Console.WriteLine($"{student.ID} - {student.Name}");
                }
            }
            Console.ReadLine();
        }
    }
}
