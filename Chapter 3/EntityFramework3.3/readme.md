# Entity Framework (Chapter 3, Lesson 3)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/3.3.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**
* **Microsoft.EntityFrameworkCore.Proxies**

## Короткий конспект

Через навігаційні властивості ми можемо завантажувати пов'язані дані. І тут у нас три стратегії завантаження:
* Eager loading (жадібне завантаження)
* Explicit loading (явне завантаження)
* Lazy loading (ліниве завантаження) (появилось з версії 2.1)

### Eager loading

Eager loading дозволяє завантажувати пов'язані дані за допомогою методу **Include()**, в який передається навігаційна властивість.
```csharp
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
 
namespace HelloApp
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Position> Positions { get; set; }
        public ApplicationContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationsdb;Trusted_Connection=True;");
        }
    }
 
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<User> Users { get; set; }
    }
    // должность пользователя
    public class Position
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
        public  Company Company { get; set; }
        public int? PositionId { get; set; }
        public Position Position { get; set; }
    }
    // страна компании
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CapitalId { get; set; }
        public City Capital { get; set; }  // столица страны
        public List<Company> Companies { get; set; }
    }
    // столица страны
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
```
Після того як ми ініціалізували БД, ми можемо отримати з неї користувачів та підтягнути до користувача компанію в якій він працює, за допомогою
метода **Include()** (підтягуємо сполучні свойства *(Company)* та *(Position)*), проте в *Company* є сполучні свойства такі як *Country* а в Country
в свою чергу є *City*, тому щоб отримати всі ці пов'язані елементи, які є включаються в основний елемент User.Company використовуємо метод **ThenInclude()**:
```csharp
var users = db.Users
                    .Include(u=>u.Company)  // добавляем данные по компаниям
                        .ThenInclude(comp => comp.Country)      // к компании добавляем страну 
                            .ThenInclude(count => count.Capital)    // к стране добавляем столицу
                    .Include(u=>u.Position) // добавляем данные по должностям
                .ToList();

foreach (var user in users)
{
    Console.WriteLine($"{user.Name} - {user.Position.Name}");
    Console.WriteLine($"{user.Company?.Name} - {user.Company?.Country.Name} - {user.Company?.Country.Capital.Name}");
    Console.WriteLine("----------------------");     // для красоты
}
```

### Explicit loading

Explicit loading передбачає явне завантаження даних за допомогою методу Load:
```csharp
using (ApplicationContext db = new ApplicationContext())
{
    Company company = db.Companies.FirstOrDefault();
    db.Users.Where(p => p.CompanyId == company.Id).Load();
     
    Console.WriteLine($"Company: {company.Name}");
    foreach (var p in company.Users)
        Console.WriteLine($"User: {p.Name}");
}
```
В даному випадку ми загружаємо тільки тих користувачів, які пов'язані з компанією. Якщо нам потрібно завантажити всіх користувачів, потрібно використати 
**db.Users.Load()**.

Для завантаження пов'язаних даних ми також можемо використовувати методи **Collection()** і **Reference()**. 
Метод **Collection()** застосовується, якщо навігаційна властивість представляє колекцію:
```csharp
using (ApplicationContext db = new ApplicationContext())
{
    Company company = db.Companies.FirstOrDefault();
    db.Entry(company).Collection(t=>t.Users).Load();
     
    Console.WriteLine($"Company: {company.Name}");
    foreach (var p in company.Users)
        Console.WriteLine($"User: {p.Name}");
}
```
Якщо навігаційна властивість представляє одиночний об'єкт, то можна застосовувати метод **Reference()**:
```csharp
using(ApplicationContext db = new ApplicationContext())
{
    User user = db.Users.FirstOrDefault();
    db.Entry(user).Reference(x => x.Company).Load();
    Console.WriteLine($"{user.Name} - {user?.Company.Name}");
}
```

### Lazy loading

Lazy loading передбачає неявне автоматичне завантаження пов'язаних даних. Однак тут є ряд умов:

* При конфігурації контексту даних потрібно викликати метод **UseLazyLoadingProxies()**
* Всі навігаційні властивості повинні бути визначені як віртуальні (тобто з модифікатором virtual), при цьому самі класи моделей повинні бути відкриті для наслідування

Також перед використанням потрібно добавити пакет **Microsoft.EntityFrameworkCore.Proxies** в проект.

```csharp
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
 
namespace HelloApp
{
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
            optionsBuilder
                .UseLazyLoadingProxies()
                .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationsdb;Trusted_Connection=True;");
        }
    }
 
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
        public virtual List<User> Users { get; set; }
    }
 
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
 
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
    }
}
```
Завантажити користувачів та всю інформацію яка з ними пов'язана можна наступним чином:
```csharp
var usersLazy = db.Users.ToList();
    foreach (var user in usersLazy)
        Console.WriteLine($"{user.Name} - " +
                          $"{user.Company?.Name} - " +
                          $"{user.Company?.Country?.Name} - " +
                          $"{user.Company?.Country?.Capital?.Name} - " +
                          $"{user.Position?.Name}");

var company = db.Companies.FirstOrDefault();
foreach (var user in company.Users)
    Console.WriteLine(user);
```

Однак при використанні lazy loading слід враховувати ряд моментів. Зокрема, при завантаженні об'єктів з бд, 
завантажуються також усі пов'язані з ними дані. Для визначення чи завантажені дані, 
EF Core використовує спеціальний прапор, який встановлюється після завантаження. 
І після цього дані не перезавантажуються, навіть якщо базі даних відбулися якісь зміни (наприклад, інший користувач змінив дані).

Lazy loading використовує синхронне завантаження. 
Тому, якщо виникне необхідність виконувати асинхронне завантаження об'єктів, 
то для цього краще застосовувати eager або explicit loading.