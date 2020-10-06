# Entity Framework Core (Chapter 5, Lesson 7)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.7.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розіді я розгялнув способи роботи з даними, без зберігання їх в кеш пам'яті. Це можна виконати за допомогою метода **AsNoTracking()**.

Запити можуть бути такими, які відстежуються та не відстежуються. За замовчуванням всі запити, які повертають об'єкти класів моделей відстежуються. 
Коли контекст даних отримує дані з бази даних, Entity Framework поміщає витягнуті об'єкти в кеш і відстежує зміни, які відбуваються з 
цими об'єктами аж до використання методу **SaveChanges()**/**SaveChangesAsync()**, який фіксує всі зміни в базі даних. Але нам не завжди 
необхідно відстежувати зміни. Наприклад, нам треба просто вивести дані для перегляду.

Щоб дані не поміщались в кеш, застосовується метод **AsNoTracking()**. Цей метод застосовується до набору IQueryable. 
При його застосуванні дані які отримуються з запиту не кешуються. Тобто запит не відслідковується. А це означає, що Entity 
Framework не виробляє якусь додаткову обробку і не виділяє додаткове місце для зберігання вилучених з БД об'єктів. І тому такі запити працюють швидше.

Розглянемо використання цього методу на роботі з подібними моделями:

```csharp
class Company
{
    [Key]
    public int CompanyID { get; set; }
    public string Name { get; set; }

    public List<Phone> Phones { get; set; }

    public Company() { Phones = new List<Phone>(); }
    public Company(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException();

        this.Name = Name; Phones = new List<Phone>();
    }
}
class Phone
{
    [Key]
    public int PhoneID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public int CompanyID { get; set; }
    public Company Company { get; set; }

    public Phone() { }
    public Phone(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException();
        this.Name = Name;
    }
    public Phone(string Name, int Price) : this(Name)
    {
        if (Price < 0)
            throw new ArgumentException();
        this.Price = Price;
    }
    public Phone(string Name, int Price, Company Company) : this(Name, Price)
    {
        this.Company = Company;
    }
    public override string ToString()
    {
        return $"Phone ID: {this.PhoneID}\n\t" +
                $"Name: {this?.Name}\n\t" +
                $"Price: {this?.Price}\n\t" +
                $"Company: {this?.Company?.Name}";
    }
}
```

Та контексту для роботи з цими моделями:

```csharp
class ApplicationContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Company> Companies { get; set; }

    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.7;Trusted_Connection=True;");
    }
}
```

Нехай в базі даних вже існує декілька моделей телефонів певних компаній:

```csharp
var apple = new Company("Apple");
var samsung = new Company("Samsung");
var xiaomi = new Company("Xiaomi");

var phone1 = new Phone("iPhone X", 799, apple);
var phone2 = new Phone("iPhone 11", 849, apple);
var phone3 = new Phone("Samsung Galaxy S9", 699, samsung);
var phone4 = new Phone("Xiaomi Redmi 4X", 100, xiaomi);
```

При звичайному виклику цих даних з БД ми витягнемо та покладемо їх в кеш пам'ять, після чого, вони будуть відстежуватись:

```csharp
using (ApplicationContext db = new ApplicationContext())
{
    var phone = db.Phones.FirstOrDefault(); // phone.Price == 799
    phone.Price = 66000; // phone.Price == 66000
    db.SaveChanges(); // В такому випадку не є обо'язковим
    
    var phones = db.Phones.ToList();
    foreach (var p in phones)
        Console.WriteLine($"{p.Name} ({p.Price})");
}
```

Ми бачимо, що в наборі phones перший елемент має у властивості Price значення 66000.

Причому в даному випадку ми можемо і не викликати метод **SaveChanges**, елемент вже і так буде закешований. 
Метод **SaveChanges** необхідний, щоб застосувати всі зміни для об'єктів в базі даних.

Але якби ми використовували **AsNoTracking()**, то результат був би інший:

```csharp
using (ApplicationContext db = new ApplicationContext())
{
    var phone = db.Phones.AsNoTracking().FirstOrDefault(); // phone.Price == 66000
    phone.Price = 33000; // phone.Price == 66000
    db.SaveChanges(); // Нічого не виконує, бо ми не відстежуємо зміни
    
    var phones = db.Phones.AsNoTracking().ToList();
    foreach (var p in phones)
        Console.WriteLine($"{p.Name} ({p.Price})");
}
```

Так як при отриманні першого елемента використовується **AsNoTracking**, він не буде відслідковуватися, і тому виклик *db.SaveChanges()* ніяк не вплине на базу даних, 
а перший елемент збереже своє первинне значення у властивості Price.

Крім використання методу **AsNoTracking**, можна вимкнути трекінг в цілому для об'єкта контексту. Для цього треба встановити значення 
*QueryTrackingBehavior.NoTracking* для властивості *db.ChangeTracker.QueryTrackingBehavior*:

```csharp
public static void PrintAllPhonesWithoutGettingThem()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Вказуємо, що для всього об'єкту контексту не потрібно виконувати відстеження
        // Всі елементи не поміщаються в кеш, як наслідок, будь які зміни не виконуються
        db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        
        foreach (var phone in db.Phones.Include(p => p.Company))
            Console.WriteLine(phone);

        // Відстежуємо кількість елементів які зараз зберігаються в кеш пам'яті
        Console.WriteLine($"Count: {db.ChangeTracker.Entries().Count()}"); // 0
    }
}
```

Загалом через властивість **ChangeTracker** ми можемо керувати відстеженням об'єктом і отримувати різноманітну інформацію. 
Наприклад, ми можемо дізнатися, скільки об'єктів відстежується в поточний момент:

```csharp
public static void PrintAllPhonesWithGettingThem()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        foreach (var phone in db.Phones.Include(p => p.Company))
            Console.WriteLine(phone);

        // Відстежуємо кількість елементів які зараз зберігаються в кеш пам'яті
        // В даному випалку ми відстежуємо всі підтягнуті об'єкти, тому значення яке виведеться буде більше 0
        Console.WriteLine($"Count: {db.ChangeTracker.Entries().Count()}"); // db.Phones.Count() + db.Company.Count()
    }
}
```