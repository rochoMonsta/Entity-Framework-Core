# Entity Framework Core (Chapter 3, Lesson 7)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/3.7.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цій темі я розглянув як можна використовувати вкладені типи без створення окремих моделей під них. Раніше якщо ми використовували вкладений тип, 
він зберігався в окремій моделі а в самому класі де був оголошений вкладений тип зберігався тільки клуч-посилання на елемент з моделі вкладеного типу.

Тепер ми можемо використовувати той самий функціонал, правда вся інформація яка зберігається в публічних полях наших моделей буде збарігатися в одній таблиці. 
По суті, ми просто розбиваємо наш клас на **Головний** та **Залежні** класи. Це можна зробити за допомогою атрибуту **[Owned]** та засобів **Fluent APi**:

За допомогою атрибуту **Owned**:
```csharp
using Microsoft.EntityFrameworkCore;
 
namespace HelloApp
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserProfile Profile { get; set; }
    }
    [Owned]
    public class UserProfile
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class ApplicationContext : DbContext
    {
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
}
```
В такому випадку модель (таблиця) Users буде містити в собі інформацію з всіх публічних свойств головного та залежних класів.

Додавати елементи в таку таблицю можна наступним чином:
```csharp
var user2 = new User()
{
    Login = "Mia12072000",
    Password = "thesunofthesun",
    UserProfile = new UserProfile() 
    {
        Name = "Mia",
        Surname = "Sorokotyaha",
        Age = 20
    }
    ///...
    public static void AddNewUser(params User[] users)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (var user in users)
                {
                    if (!db.Users.Any(u => u.Login == user.Login && u.Password == user.Password))
                    {
                        db.Users.Add(user);
                        db.SaveChanges();
                    }
                }
            }
        }
};
```
* За допомогою засобів Fluent APi та методу **OwnsOne()**:
```csharp
public class ApplicationContext : DbContext
{
    ///...
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Вказуємо навігаційне свойство яке представляє собою залежний тип
        modelBuilder.Entity<User>().OwnsOne(u => u.Profile);
    }
}
```
### Приватні типи

Цілком можливо, що ми захочемо зробити навігаційну властивість Profile приватною. У цьому випадку знадобиться додаткове налаштування:
```csharp
using Microsoft.EntityFrameworkCore;
 
namespace HelloApp
{
    public class User
    {
        public User()  { }
        public User(string login, string password, UserProfile profile)
        {
            Login = login; Password = password; Profile = profile;
        }
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        private UserProfile Profile { get; set; }
        public override string ToString()
        {
            return $"Name: {Profile?.Name}  Age: {Profile?.Age}  Login: {Login} Password: {Password}";
        }
    }
    public class UserProfile
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
    public class ApplicationContext : DbContext
    {
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
            modelBuilder.Entity<User>().OwnsOne(typeof(UserProfile), "Profile");
        }
    }
}
```
### Вкладені власні типи

Одні власні типи можуть мати інші власні типи, збільшуючи складність структури класів. Наприклад, визначимо наступні класи:
```csharp
public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public UserProfile Profile { get; set; }
}  
public class UserProfile
{
    public Claim Name { get; set; }
    public Claim Age { get; set; }
}
public class Claim
{
    public string Key { get; set; }
    public string Value { get; set; }
}
```
Головною сутністю тут є клас User, який містить об'єкт класу UserProfile. Клас UserProfile, в свою чергу, 
також може містити записи ще одного класу - Claim, який представляє окремі дані за принципом ключ-значення.

Тепер визначимо контекст даних:
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
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationsdb;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().OwnsOne(u => u.Profile, p =>
        {
            p.OwnsOne(c => c.Name);
            p.OwnsOne(c => c.Age);
        });
    }
}
```
Визначити елемент подібної взаємодії можна наступним чином:
```csharp
var user1 = new User()
{
    Login = "rochoMonsta",
    Password = "lovenotfound",
    UserProfile = new UserProfile() 
    {
        Name = new Claim() { Name = "Name", Value = "Roman" },
        Surname = new Claim() { Name = "Surname", Value = "Cholkan" },
        Age = new Claim() { Name = "Age", Value = "20" }
    }
};
```