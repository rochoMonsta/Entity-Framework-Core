# Entity Framework (Chapter 2, Lesson 11)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.14.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

Починаючи з версії 2.1 в Entity Framework Core була додана можливість ініціалізовувати базу даних при її створенні деякими початковими даними.
Для ініціалізації БД в класі контексту даних необхідно використовувати метод **OnModelCreating** наприклад:
```csharp
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
 
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=helloappdb5;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User[] 
            {
                new User { Id=1, Name="Tom", Age=23},
                new User { Id=2, Name="Alice", Age=26},
                new User { Id=3, Name="Sam", Age=28}
            });
    }
}
```
В екзепляра класу **ModelBuilder** викликаємо метод **Entity<T>()**, який типізуємо тим типом даних, в який будемо передавати початкові значення.
В даному випадку клас User - таблиця користувачів. Потім викликаємо метод **HasData** в який передаємо користувачів який потрібно додати в БД
на етапі створення БД та таблиці. Важливим є те, що в такому випадку потрібно в ручну ініціалізовувати PK.

При цьому слід враховувати, що ініціалізація початковими даними буде виконуватися тільки в двох випадках:

* При виконанні міграції. (При створенні міграції додаються дані автоматично включаються в скрипт міграції)
* При виклику методу **Database.EnsureCreated()**, який створює БД при її відсутності

