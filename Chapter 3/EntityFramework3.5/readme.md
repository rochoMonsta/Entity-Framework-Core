# Entity Framework (Chapter 3, Lesson 5)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/3.5.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

Зв'язок один-до-багатьох (one-to-many) представляє ситуацію, коли одна модель зберігає посилання на один об'єкт іншої моделі, 
а друга модель може посилатися на колекцію об'єктів першої моделі. Наприклад, в одній компанії може працювати кілька співробітників, 
а кожен співробітник у свою чергу може офіційно працювати тільки в одній компанії:
```csharp
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<User> Users { get; set; } // сотрудники компании
}
 
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
 
    public int CompanyId { get; set; }
    public Company Company { get; set; }  // компания пользователя
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
Додавання користувачів та компаній:
```csharp
public static void AddUser(User user)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (db.Companies.Any(c => c.Name == user.Company.Name))
        {
            user.CompanyID = db.Companies.First(c => c.Name == user.Company.Name).CompanyID;
            user.Company = null;
        }
        if (!db.Users.Any(u => u.Name == user.Name))
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
    }
}
public static void AddUser(params User[] users)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        foreach (var user in users)
            AddUser(user);
    }
}
```

У випадку відношення один до багатьох, при видалені члену який відноситься до багатьох, всі елементи які на нього зсилались будуть видалені,
наприклад **(видалити компанію, як наслідок всі працівники цієї компанії будуть видалені)**. У випадку якщо видалити член який посилається на основну частину - основна
частина не постраждає, наприклад **(видалити працівника компанії, як наслідок, компанія не зміниться, видалиться тільки працівник)**.
Видалення користувача:
```csharp
public static void DeleteUser(User user)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (db.Users.Any(u => u.Name == user.Name))
        {
            var deletedUser = db.Users.First(u => u.Name == user.Name);
            db.Users.Remove(deletedUser);
            db.SaveChanges();
        }
    }
}
```
Видалення компанії:
```csharp
public static void DeleteCompany(Company company)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (db.Companies.Any(c => c.Name == company.Name))
        {
            var deletedCompany = db.Companies.First(c => c.Name == company.Name);
            db.Companies.Remove(deletedCompany);
            db.SaveChanges();
        }
    }
}
```

Виведення працівників:
```csharp
public static void PrintCompaniesWorker()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        foreach (var company in db.Companies.Include(c => c.Users))
        {
            Console.WriteLine($"Company: {company.Name}; ID: {company.CompanyID};");

            foreach (var user in company.Users)
                Console.WriteLine($"\tUser ID: {user.UserID}; Name: {user.Name}; Company id: {user.CompanyID};");
            Console.WriteLine();
        }
    }
}
```