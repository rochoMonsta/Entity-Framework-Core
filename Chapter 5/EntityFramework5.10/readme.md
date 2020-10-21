# Entity Framework Core (Chapter 5, Lesson 10)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.9.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розділі я розглянув, як можна створити внутрішній фільтр який буде діяти для всередині контексту будь який раз, як той буде використовуватись.

У Entity Framework Core 2.0 була додана така функціональність як фільтри запитів рівня моделі (Model-level query filters). 
Вона дозволяє визначити предикат запиту LINQ безпосередньо в метаданих моделі (зазвичай в методі **OnModelCreating** контексту даних). 
Такі фільтри автоматично застосовуються до будь-яких запитів LINQ, в яких використовуються класи, для яких визначено фільтр.

Нехай у нас визначені наступні класи:

```csharp
class User
{
    [Key]
    public int UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }

    public int RoleID { get; set; }
    public Role Role { get; set; }

    public User() { }
    public User(string FirstName, string LastName, int Age)
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
            throw new ArgumentNullException(nameof(FirstName) + " " + nameof(LastName));

        if (Age > 100 || Age < 0)
            throw new ArgumentException(nameof(Age));
        this.FirstName = FirstName; this.LastName = LastName; this.Age = Age;
    }
    public User(string FirstName, string LastName, int Age, Role Role) : this(FirstName, LastName, Age)
    {
        this.Role = Role;
    }
    public override string ToString()
    {
        return $"User ID: {this.UserID}\n\t" +
                $"Name: {this.FirstName}\n\t" +
                $"Surname: {this.LastName}\n\t" +
                $"Age: {this.Age}\n\t" +
                $"Role: {this?.Role?.Name}\n";
    }
}

class Role
{
    [Key]
    public int RoleID { get; set; }
    public string Name { get; set; }

    public Role() { }
    public Role(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException(nameof(Name));
        this.Name = Name;
    }
}
```

Також визначимо контекст даних:

```csharp
class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public ApplicationContext() => Database.EnsureCreated();

    public int RoleID { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.10;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasQueryFilter(u => u.Age > 10 && u.RoleID == this.RoleID);
    }
}
```

У метод **HasQueryFilter()** передається предикат, яким повинен задовольняти об'єкт User, щоб бути витягнутим з бази даних. 
Тобто в результаті запитів будуть вилучатись тільки ті об'єкти User, у яких значення властивості *Age* більше 17, а властивість 
*RoleID* дорівнює значенню властивості *RoleID* їх контексту даних.

Використовуємо дані класи:

```csharp
var admin = new Role("Admin");
var user = new Role("User");

var person1 = new User("Roman", "Cholkan", 20, admin);
var person2 = new User("Mia", "Sorokotyaha", 20, admin);
var person3 = new User("Edward", "Black", 19, user);
var person4 = new User("Jack", "Sparrow", 54, user);
var person5 = new User("Harry", "Potter", 15, user);
var person6 = new User("Darth", "Vader", 78, admin);

...
// Метод, який виводить дані з бд користувачів підключаючи до них дані з бд ролей

public static void PrintAllUser(int roleID) // roleID - індекс ролі в бд ролей
{
    // Створюємо об'єкт контексту даних передаючи в його свойство значення номеру ролі,
    // як наслідок, в циклі будуть перебиратись тільки ті елементи, які задовільняють
    // внутрішній фільтр ApplicationContext для цих даних.

    using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
    {
        foreach (var user in db.Users.Include(u => u.Role))
            Console.WriteLine(user);
    }
}
```

Result:

```csharp
PrintAllUser(1);
// Roman Cholkan 20 Admin
// Mia Sorokotyaha 20 Admin
// Darth Vader 78 Admin
```

Тобто тільки три об'єкти з доданих шести відповідають тому предикату, який був переданий у HasQueryFilter. І цей фільтр буде діяти для всіх запитів 
до бази даних, які витягають дані з таблиці Users. Наприклад, знаходження мінімального віку:

```csharp
// Виконуємо пошук наймолодшого користувача серед тих, які задовільняють внутрішній фільтр ApplicationContext для цих даних.
public static User FindUserWithMinAge(int roleID)
{
    using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
    {
        var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Age == db.Users.Min(u => u.Age));

        return user == null ? null : user;
    }
}
...
FindUserWithMinAge(2);
// Result:
// Harry Potter 15 User
```

Якщо необхідно під час запиту відключити фільтр, то застосовується метод **IgnoreQueryFilters()**:

```csharp
public static User FindUserWithMinAge(int roleID)
{
    using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
    {
        var user = db.Users.Include(u => u.Role).FirstOrDefault(u => u.Age == db.Users.Min(u => u.Age));

        return user == null ? null : user;
    }
}

public static User FindUserWithMinAge(int roleID, bool ignoreFilters)
{
    using (ApplicationContext db = new ApplicationContext() { RoleID = roleID })
    {
        if (ignoreFilters)
        {
            // Ігноруємо внутрішній фільтр, як наслідок, пошук наймолодшого користувача перевіряє всіх користувачів.
            var user = db.Users.Include(u => u.Role).IgnoreQueryFilters().FirstOrDefault(u => u.Age == db.Users.Min(u => u.Age));

            return user == null ? null : user;
        }
        else
            return FindUserWithMinAge(roleID);
    }
}
```