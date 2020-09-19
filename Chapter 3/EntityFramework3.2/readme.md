# Entity Framework (Chapter 3, Lesson 2)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/3.2.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

Каскадне видалення представляє автоматичне видаляння залежних елементів при видалені батьківських елементів.
По замовчуванню воно викликається в тому випадку, якщо є певна залежність між моделями:
```csharp
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } // название компании
     
    public List<User> Users { get; set; }
}
 
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
 
    public int CompanyId { get; set; } // внешний ключ
    public Company Company { get; set; }    // навигационное свойство
}
public class ApplicationContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationsdb;Trusted_Connection=True;");
    }
}
```
В даному випадку в користувача свойство CompanyId - є NOT NULL, тобто обов'язкове до заповнення.
Залежність побудована таким чином, що основна модель - Company, а залежна від неї User. Тому при видалені однієї з компаній в таблиці Companies - будуть
видалятись всі залежні від цієї компанії користувачі:
```csharp
using (ApplicationContext db = new ApplicationContext())
{
    // добавляем начальные данные
    Company microsoft = new Company { Name = "Microsoft" };
    Company google = new Company { Name = "Google" };
    db.Companies.AddRange(microsoft, google);
    db.SaveChanges();
    User tom = new User { Name = "Tom", Company = microsoft };
    User bob = new User { Name = "Bob", Company = google };
    User alice = new User { Name = "Alice", Company = microsoft };
    User kate = new User { Name = "Kate", Company = google };
    db.Users.AddRange(tom, bob, alice, kate);
    db.SaveChanges();
 
 
    // получаем пользователей
    var users = db.Users.ToList();
    foreach (var user in users) Console.WriteLine($"{user.Name}");
 
    // Удаляем первую компанию
    var comp = db.Companies.FirstOrDefault();
    db.Companies.Remove(comp);
    db.SaveChanges();
    Console.WriteLine("\nСписок пользователей после удаления компании");
    // снова получаем пользователей
    users = db.Users.ToList();
    foreach (var user in users) Console.WriteLine($"{user.Name}");
}
```

Ми можемо змінити цю залежність, змінивши зовнішній ключ:
```csharp
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } // название компании
     
    public List<User> Users { get; set; }
}
 
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
 
    public int? CompanyId { get; set; } // внешний ключ
    public Company Company { get; set; }    // навигационное свойство
}
```
В даному випадку **public int? CompanyId { get; set; }** стає **Nullable<int>**, тобто допускає значення *null*.
В такому випадку, при видалені одного елементу з таблички Companies - всі користувачі які були закріплені за цією компанією
залишаться, просто замість індесу відповідної компанії в стовпці **CompanyId** буде *null*.

### Налаштування каскадного видалення за допомогою Fluent API

У Fluent API доступні три різних сценарія, які керують поведінкою залежною сутності в разі видалення головною сутності:

* Cascade: залежна сутність видаляється разом з головною
* SetNull: властивість-зовнішній ключ в залежній суті отримує значення null при видалені головного елементу
* Restrict: залежна сутність не змінюється при видаленні головної суті

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.Generic;
 
 
public class ApplicationContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationsdb;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(p => p.Company)
            .WithMany(t => t.Users)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
 
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
 
    public List<User> Users { get; set; }
}
 
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
 
    public int? CompanyId { get; set; }
    public Company Company { get; set; }
}
```