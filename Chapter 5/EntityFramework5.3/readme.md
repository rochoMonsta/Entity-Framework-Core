# Entity Framework Core (Chapter 5, Lesson 3)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.3.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розділі я розгялнув як можна підтягнути конкретні поля та значення з пов'язаної моделі, замість підтягування всієї інформації за допомогою **Include()**.
Також я розглянув як можна відсортувати значення з БД за допомогою методів LinQ **OrderBy()** для сортування значень в порядку зростання та **OrderByDescending()** для 
сортування значень в порядку спадання.

### Проекція

Розгялнемо проекцію на прикладі таких моделей:

```csharp
class Phone
{
    [Key]
    public int PhoneID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public int CompanyID { get; set; }
    public Company Company { get; set; }

    public override string ToString()
    {
        return $"Phone ID: {this.PhoneID}\n\t" +
                $"Name: {this.Name}\n\t" +
                $"Price: {this.Price}\n\t" +
                $"Company: {this?.Company?.Name}";
    }
}

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
```

Та такого контексту:

```csharp
class ApplicationContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Company> Companies { get; set; }

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.3;Trusted_Connection=True;");
    }
}
```

Раніше для того щоб вивести назву компанії, чи будь яку іншу інформацію яка відповідє компанії потрібно було використовувати **Include()** як показано в наступному 
прикладу:

```csharp
public static void PrintAllPhones()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        foreach (var phone in db.Phones.Include(p => p.Company))
            Console.WriteLine(phone);
    }
}
```

Проте нам не завжди потрібна вся інформація, яка описує модель Company. Щоб вибрати якісь конкретні значення, ми можемо створити новий анонімний тип, та задати йому 
поля після чого їх ініціалізувати даними які нам потрібні:

```csharp
public static void PrintAllPhones()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var phoneWithCompany = db.Phones.Select(p => new
        {
            Name = p.Name,
            Price = p.Price,
            Company = p.Company.Name
        });
        foreach (var phone in phoneWithCompany)
            Console.WriteLine($"Name: {phone.Name}\n\t" +
                              $"Price: {phone.Price}\n\t" +
                              $"Company: {phone.Company}\n");
    }
}
```

В такому випадку, ми отримаємо назву компанії без вигрузки всього що пов'язано з цією компанією.

Ми також можемо використати вже заготований клас, який не буде моделю, після чого його використати замість створення анонімного класу:

```csharp
public class Model
{
    public string Name { get; set; }
    public string Company { get; set; }
    public int Price { get; set; }
}
...
public static void PrintAllPhones()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var phoneWithCompany = db.Phones.Select(p => new Model
        {
            Name = p.Name,
            Price = p.Price,
            Company = p.Company.Name
        });
        foreach (var phone in phoneWithCompany)
            Console.WriteLine($"Name: {phone.Name}\n\t" +
                              $"Price: {phone.Price}\n\t" +
                              $"Company: {phone.Company}\n");
    }
}
```

### Сортування

Для того, щоб виконати сортування даних в порядку зростання - потрібно використати **OrderBy()**, після чого, вказати умову по якій буде відбуватись сортування:

```csharp
// Сортування за зростанням ціни телефонів
public static void PhonesOrderByPrice()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var sorted = db.Phones.Select(p => new
        {
            Name = p.Name,
            Price = p.Price,
            CompanyName = p.Company.Name
        }).OrderBy(p => p.Price).ThenBy(p => p.Name).ThenBy(p => p.CompanyName);

        foreach (var phone in sorted)
            Console.WriteLine($"Name: {phone.Name}\n\t" +
                              $"Price: {phone.Price}\n\t" +
                              $"Company: {phone.CompanyName}\n");
    }
}
```

В цьому прикладі також можна помітити **ThenBy()** - це додаткова умова сортування, якщо значення які порівнюються є ідентичними. В даному випадку, якщо 
ціна телефонів співпадає, ми будемо виконувати сортування за іменем телефона (по зростанню).

Для того щоб виконати сортування за спаданням - потрібно використати **OrderByDescending()**, після чого, потрібно вказати умову сортування:

```csharp
// Сортування за спаданням імені. Додаткова умова - за спаданням імені компанії.
public static void PhonesOrderByDescendingName()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var sorted = db.Phones.Select(p => new
        {
            Name = p.Name,
            Price = p.Price,
            Company = p.Company.Name
        }).OrderByDescending(p => p.Name).ThenBy(p => p.Company);

        foreach (var phone in sorted)
            Console.WriteLine($"Name: {phone.Name}\n\t" +
                              $"Price: {phone.Price}\n\t" +
                              $"Company: {phone.Company}\n");
    }
}
```