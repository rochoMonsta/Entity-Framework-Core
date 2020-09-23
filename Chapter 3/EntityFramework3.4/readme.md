# Entity Framework Core (Chapter 3, Lesson 4)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/3.4.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

Ставлення один до одного передбачає, що головна сутність може посилатися тільки на один об'єкт залежною сутності. 
У свою чергу, залежна сутність може посилатися тільки на один об'єкт головною сутності.

Розглянемо стандартний приклад подібних відносин: є клас користувача User, який зберігає логін і пароль, то є дані облікового запису. 
А всі дані профілю, такі як ім'я, вік і так далі, виділяються в клас профілю UserProfile.
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
    public int Id { get; set; }
  
    public string Name { get; set; }
    public int Age { get; set; }
  
    public int UserId { get; set; }
    public User User { get; set; }
}
 
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
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
У зв'язку з цим між класами сутність UserProfile є залежною по відношенню до сутності User. І щоб встановити зв'язок один до одного, 
у залежній сутності встановлюється властивість зовнішній ключ: **public int UserId { get; set; }**. 
Завдяки цьому Entity Framework дізнається, що UserProfile є залежною сутністю. Наприклад, в класі User також є навігаційна властивість - посилання на об'єкт 
UserProfile, але при цьому зовнішній ключ відсутній.

Глянемо як можна працювати з подібними моделями:

* Виведення
```csharp
public static void PrintUser()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        foreach (var user in db.Users.Include(u => u.Profile))
            Console.WriteLine($"{user?.Profile?.Name} - {user?.Profile?.Age} - " +
                                $"{user?.Login} - {user?.Password}");
    }
}
```
* Редагування
```csharp
public static void ChangeUser(string login, string password, string name, int age)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var userProfile = db.UserProfiles.FirstOrDefault(u => u.User.Login == login && u.User.Password == password);
        if (userProfile != null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException();
            if (age < 0 || age > 100)
                throw new ArgumentException();

            userProfile.Name = name; userProfile.Age = age;
            db.SaveChanges();
        }
    }
}
```
* Видалення
```csharp
public static void DeleteUserProfile(string login, string password)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var userProfile = db.UserProfiles.FirstOrDefault(u => u.User.Login == login && u.User.Password == password);
        if (userProfile != null)
        {
            db.UserProfiles.Remove(userProfile);
            db.SaveChanges();
        }
    }
}
```
При видаленні треба враховувати наступне: так як об'єкт UserProfile вимагає наявність об'єкта User і залежить від цього об'єкта, 
то при видаленні пов'язаного об'єкта User також буде вилучений і пов'язаний з ним об'єкт UserProfile. Якщо ж буде видалений об'єкт 
UserProfile, на об'єкт User це ніяк не вплине.

### Налаштування відносини за допомогою Fluent API

Для настройки такого ставлення за допомогою Fluent API застосовуються методи **HasOne()** і **WithOne()**:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder
        .Entity<User>()
        .HasOne(u => u.Profile)
        .WithOne(p => p.User)
        .HasForeignKey<UserProfile>(p => p.UserKey);
}
```

### Об'єднання таблиць

Починаючи з версії 2.0 в Entity Framework Core можна зберігати дані моделей, які пов'язані ставленням один-до-одного, в одній таблиці. 
Наприклад, візьмемо ті ж моделі User і UserProfile і визначимо для них одну таблицю Users:
```csharp
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
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
            .HasOne(e => e.Profile).WithOne(e => e.User)
            .HasForeignKey<UserProfile>(e => e.Id);
        modelBuilder.Entity<User>().ToTable("Users");
        modelBuilder.Entity<UserProfile>().ToTable("Users");
    }
}
```
В цьому випадку при додаванні нам обов'язково треба додати одночасно дані обох моделей:
```csharp
using (ApplicationContext db = new ApplicationContext())
{
    User user1 = new User
    {
        Login = "login1",
        Password = "pass1234",
        Profile = new UserProfile { Age = 22, Name = "Tom"}
    };
    User user2 = new User
    {
        Login = "login2",
        Password = "5678word2",
        Profile = new UserProfile { Age = 27, Name = "Alice" }
    };
    db.Users.AddRange(new List<User> { user1, user2 });
    db.SaveChanges();
     
    var profiles = db.UserProfiles.ToList();
    foreach (var profile in profiles)
        Console.WriteLine(profile.Name);
}
```
