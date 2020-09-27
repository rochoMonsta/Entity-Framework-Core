# Entity Framework Core (Chapter 4, Lesson 1)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/4.1.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

За замовчуванням при роботі з ланцюжками успадкування класів Entity Framework Core використовує підхід TPH 
(Table Per Hierarchy / Таблиця на одну ієрархію класів). Власне на даний момент це єдина форма роботи з ієрархіями класів 
в Entity Framework Core. При використанні даного підходу TPH для всіх класів з однієї ієрархії в базі даних створюється одна таблиця. 
А щоб визначити, до якого саме класу належить рядок в таблиці, в цій же таблиці створюється додатковий стовпець - дискримінатор.

Припустимо в нас є подібна ієрархія:
```csharp
class User
{
    [Key]
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }
    public override string ToString() => $"{this.UserID}) {this.Name} {this.Surname} {this.Age}";
}
class Employee : User
{
    public int Salary { get; set; }
    public override string ToString() => base.ToString() + $" {this.Salary}";
}
class Manager : Employee
{
    public string Departament { get; set; }
    public override string ToString() => base.ToString() + $" {this.Departament}";
}
```
Та відповідний контекст:
```csharp
using EntityFramework4._1.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework4._1.Context
{
    class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Manager> Managers { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF4.1;Trusted_Connection=True;");
        }
    }
}
```

Щоб включити всі класи з ієрархії успадкування в базу даних, в контексті даних для кожного типу повинен бути визначений набір DbSet.

Згенерована база даних буде містити для всіх типів одну таблицю Users. Крім всіх властивостей класів User, Employee і Manager тут також 
з'являється ще один стовпець - **Discriminator**. Він має тип nvarchar (тобто рядок), а в якості значення він приймає назву класу, до якого належить рядок в таблиці.

