# Entity Framework Core (Chapter 5, Lesson 8)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.8.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розділі я розглянув те, як Entity Framework Core виконує запити до БД, та як виконується робота з отриматиним значеннями.

Розглянемо, як EF обробляє запити на отримання даних з БД. Спочатку вираз 
LINQ обробляються Entity Framework Core, і на їх основі створюється об'єкт запиту в тій формі, в якій він може 
оброблятися провайдером бази даних. Створений об'єкт запиту кешиється, що дозволяє не виконувати повторного створення цього запиту при повторному виклику.

Потім цей об'єкт запиту передається провайдеру бази даних, який транслює його на мову, зрозумілу для бази даних (наприклад, SQL). 
База даних обробляє запит і повертає певний результат.

EF Core отримує результат обробки, і далі його дії залежать від того, чи відстежується результати запиту чи ні.

Якщо запит відслідковується, то є два альтернативних варіанти:
* Якщо дані, отримані з бд, представляють об'єкти, які вже відслідковуються, тобто вони вже є в контексті, то EF повертає ті об'єкти, які вже є в контексті.
* Якщо дані, отримані з бд, представляють об'єкти, які ще не відслідковуються, їх немає в контексті, то EF створює за цими даними нові об'єкти, 
додає в контекст, починає їх відслідковувати і повертає їх користувачеві.

Якщо запит не відслідковується, тобто відстеження відключено за допомогою методу **AsNoTracking()** або встановлення властивості 
**ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking**, то EF створює за цими даними нові об'єкти і повертає їх користувачеві. 
На відміну від запитів які відстежуються, створені об'єкти не додаються в контекст і не відслідковуються.

Розглянемо все це на основі моделі User та контексту ApplicationContext:

```csharp
class User
{
    [Key]
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }

    public User() { }
    public User(string Name, string Surname, int Age)
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
            throw new ArgumentNullException();

        if (Age < 0 || Age > 100)
            throw new ArgumentException();

        this.Name = Name; this.Age = Age; this.Surname = Surname;
    }

    public override string ToString()
    {
        return $"User ID: {this.UserID}\n\t" +
                $"Name: {this.Name}\n\t" +
                $"Surname: {this.Surname}\n\t" +
                $"Age: {this.Age}";
    }
}

class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.8;Trusted_Connection=True;");
    }
}
```

В такому випадку, запит, який відслідковуються буде виглядати наступним чином:

```csharp
var user1 = new User() { Name = "Roman", Surname = "Cholkan", Age = 20 };
ChangeUserWithTracking("Edward", "Balck", 19);
...
public static void ChangeUserWithTracking(string name, string surname, int age) 
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var user1 = db.Users.FirstOrDefault(); // Roman, Cholkan, 20
        var user2 = db.Users.FirstOrDefault(); // Roman, Cholkan, 20

        Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" + // Roman, Cholkan, 20
                            $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}"); // Roman, Cholkan, 20

        user1.Name = name; user1.Surname = surname; user1.Age = age; // Edward, Balck, 19

        Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" + // Edward, Balck, 19
                            $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}"); // Edward, Balck, 19
    }
}
```

Так як запит **db.Users.FirstOrDefault()** відслідковується, то при отриманні даних, по ним буде створюватися об'єкт user1, 
який додається в контекст і починає відслідковуватися.

Далі повторно викликається даний запит для отримання об'єкта user2. Цей запит той же що й відслідковується, тому EF побачить, 
що такий об'єкт уже є в контексті, він уже відстежується, і поверне посилання на той же об'єкт. Тому всі зміни зі змінною user1 торкнуться і змінну user2.

Розглянемо інший приклад, коли ми не відслідковуємо значення:

```csharp
var user1 = new User() { Name = "Roman", Surname = "Cholkan", Age = 20 };
ChangeUserWithoutTracking("Edward", "Balck", 19);
...
public static void ChangeUserWithoutTracking(string name, string surname, int age)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var user1 = db.Users.FirstOrDefault(); // Roman, Cholkan, 20
        var user2 = db.Users.AsNoTracking().FirstOrDefault(); // Roman, Cholkan, 20

        Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" + // Roman, Cholkan, 20
                            $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}"); // Roman, Cholkan, 20

        user1.Name = name; user1.Surname = surname; user1.Age = age; // Edward, Balck, 19

        Console.WriteLine($"User1: {user1.Name + " " + user1.Surname + " " + user1.Age}\n" + // Edward, Balck, 19
                            $"User2: {user2.Name + " " + user2.Surname + " " + user2.Age}"); // Roman, Cholkan, 20
    }
}
```

З першим об'єктом user1 все як і раніше: він також потрапляє в контекст і відстежується. А ось другий запит тепер є таким, який не відслідковується, 
так як використовує метод **AsNoTracking()**. Тому для даних, отриманих в результаті цього запиту, буде створюватися новий об'єкт, 
який ніяк не буде пов'язаний з об'єктом user1. І зміни одного з цих об'єктів ніяк не вплинуть на другий об'єкт.