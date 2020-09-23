# Entity Framework Core (Chapter 3, Lesson 6)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/3.6.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

Ще одним способом асоціації об'єктів є зв'язок багато-до-багатьох (many-to-many). 
Прикладом такого ставлення може служити відвідування студентами університетських курсів. Один студент може відвідувати відразу кілька курсів, і, 
в свою чергу, один курс може відвідувати безліччю студентів.

```csharp
public class ApplicationContext : DbContext
{
    public DbSet<Course> Courses { get; set; }
    public DbSet<Student> Students { get; set; }
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StudentCourse>()
            .HasKey(t => new { t.StudentId, t.CourseId });
 
        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Student)
            .WithMany(s => s.StudentCourses)
            .HasForeignKey(sc => sc.StudentId);
 
        modelBuilder.Entity<StudentCourse>()
            .HasOne(sc => sc.Course)
            .WithMany(c => c.StudentCourses)
            .HasForeignKey(sc => sc.CourseId);
    }
     
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationsdb;Trusted_Connection=True;");
    }
}
 
public class Course
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StudentCourse> StudentCourses { get; set; }
     
    public Course()
    {
        StudentCourses = new List<StudentCourse>();
    }
}
public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<StudentCourse> StudentCourses { get; set; }
     
    public Student()
    {
        StudentCourses = new List<StudentCourse>();
    }
}
public class StudentCourse
{
    public int StudentId { get; set; }
    public Student Student { get; set; }
 
    public int CourseId { get; set; }
    public Course Course { get; set; }
}
```
На даний момент (версія EF Core 3.0) Entity Framework Core не дозволяє створювати відношення багато-до-багатьох без сполучною сутності. 
У нашому випадку такою сутністю є клас StudentCourse. Фактично зв'язок багато-до-багатьох тут розбивається на два зв'язки один-до-багатьох: 
один Student - багато StudentCourse, один Course - багато StudentCourse.

Додавання та Виведення:
```csharp
using (ApplicationContext db = new ApplicationContext())
{
    Student s1 = new Student { Name = "Tom"};
    Student s2 = new Student { Name = "Alice" };
    Student s3 = new Student { Name = "Bob" };
    db.Students.AddRange(new List<Student> { s1, s2, s3 });
 
    Course c1 = new Course { Name = "Алгоритмы" };
    Course c2 = new Course { Name = "Основы программирования" };
    db.Courses.AddRange(new List<Course> { c1, c2 });
 
    db.SaveChanges();
 
    // добавляем к студентам курсы
    s1.StudentCourses.Add(new StudentCourse { CourseId = c1.Id, StudentId = s1.Id });
    s2.StudentCourses.Add(new StudentCourse { CourseId = c1.Id, StudentId = s2.Id });
    s2.StudentCourses.Add(new StudentCourse { CourseId = c2.Id, StudentId = s2.Id });
    db.SaveChanges();
 
    var courses = db.Courses.Include(c => c.StudentCourses).ThenInclude(sc => sc.Student).ToList();
    // выводим все курсы
    foreach (var c in  courses)
    {
        Console.WriteLine($"\n Course: {c.Name}");
        // выводим всех студентов для данного кура
        var students = c.StudentCourses.Select(sc => sc.Student).ToList();
        foreach (Student s in students)
            Console.WriteLine($"{s.Name}");
    }
}
```

Редагування:
```csharp
// удаление курса у студента
Student student = db.Students.Include(s=>s.StudentCourses).FirstOrDefault(s => s.Name == "Alice");
Course course = db.Courses.FirstOrDefault(c => c.Name == "Алгоритмы");
if (student != null && course != null)
{
    var studentCourse = student.StudentCourses.FirstOrDefault(sc => sc.CourseId == course.Id);
    student.StudentCourses.Remove(studentCourse);
    db.SaveChanges();
}
```
Видалення ж студента або курсу з бази даних призведе до того, що всі рядки в таблиці StudentCourses, які містять id об'єкта який видаляється, також будуть видалені:
Видалення:
```csharp
Student student = db.Students.FirstOrDefault();
db.Students.Remove(student);
db.SaveChanges();
```