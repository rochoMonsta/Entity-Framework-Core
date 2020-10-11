# Entity Framework Core (Chapter 5, Lesson 9)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.6.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розділі я розглянув відміності між такими інтерфейсами як **IEnumerable** та **IQueryable**.

При виклику методів LINQ ми тільки створюємо запит. Його безпосереднє виконання відбувається, коли ми починаємо 
споживати результати цього запиту. Нерідко це відбувається при переборі результату запиту в циклі for або при 
застосуванні до нього ряду методів - ToList або ToArray, а також якщо запит являє скалярний значення, наприклад, метод Count.

В процесі виконання запитів LINQ to Entities ми може отримувати два об'єкти, які надають набори даних: IEnumerable і IQueryable. 
З одного боку, інтерфейс IQueryable успадковується від IEnumerable, тому по ідеї об'єкт IQueryable це і є також об'єкт IEnumerable. 
Але реальність трохи складніше. Між об'єктами цих інтерфейсів є різниця в плані функціональності, тому вони не взаємозамінні.

Інтерфейс IEnumerable знаходиться в просторі імен System.Collections і System.Collections.Generic (узагальнена версія). 
Об'єкт IEnumerable представляє набір даних в пам'яті і може переміщатися за цими даними тільки вперед. Запит, поданий 
об'єктом IEnumerable, виконується негайно і повністю, тому отримання даних відбувається швидко.

При виконанні запиту IEnumerable завантажує всі дані, і якщо нам треба виконати їх фільтрацію, то сама фільтрація відбувається на стороні клієнта.

Інтерфейс IQueryable розташовується в просторі імен System.Linq. Об'єкт IQueryable надає віддалений доступ до бази даних і дозволяє переміщатися 
за даними як в прямому порядку від початку до кінця, так і в зворотному порядку. У процесі створення запиту, що повертається об'єктом якого є 
IQueryable, відбувається оптимізація запиту. У підсумку в процесі його виконання витрачається менше пам'яті, менше пропускної здатності мережі, 
але в той же час він може оброблятися трохи повільніше, ніж запит, який повертає об'єкт IEnumerable.

Для прикладу використаємо подібну модель та контекст:

```csharp
class User
{
    [Key]
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int Age { get; set; }

    public User() { }
    public User(string Name, string Surname)
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname))
            throw new ArgumentNullException();
        this.Name = Name; this.Surname = Surname;
    }
    public User(string Name, string Surname, int Age) : this(Name, Surname)
    {
        if (Age < 0 || Age > 100)
            throw new ArgumentException();
        this.Age = Age;
    }
    public override string ToString()
    {
        return $"Person ID {this?.UserID}\n\t" +
                $"Name: {this?.Name}\n\t" +
                $"Surname: {this?.Surname}\n\t" +
                $"Age: {this?.Age}";
    }
}
...
class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.9;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User { UserID = 1, Name = "Roman", Surname = "Cholkan", Age = 20 },
            new User { UserID = 2, Name = "Emma", Surname = "Stone", Age = 19 },
            new User { UserID = 3, Name = "William", Surname = "Balck", Age = 29 },
            new User { UserID = 4, Name = "Robert", Surname = "Doms", Age = 61 }
        );

    }
}
```

Візьмемо два начебто ідентичних вираження. Об'єкт IEnumerable:

```csharp
int id = 1;
IEnumerable<User> userIEnum = db.Users;
var users=userIEnum.Where(p => p.UserID > id).ToList();
```

В такому випадку запит буде виглядяти наступним чином:

```SQL
SELECT [u].[Id], [u].[Name]
FROM [Users] AS [u]
```

Фільтрація результату, позначена за допомогою методу Where (p => p.Id> id) буде йти вже після вибірки з бд в самому додатку.

Щоб поєднати фільтри, нам треба було відразу застосувати метод Where: db.Users.Where (p => p.Id> id);

Объект IQueryable:

```csharp
int id = 1;
IQueryable<User> userIQuer = db.Users;
var users=userIQuer.Where(p => p.UserID > id).ToList();
```

В такому випадку запит буде виглядяти наступним чином:

```SQL
SELECT [u].[Id], [u].[Name]
FROM [Users] AS [u]
WHERE [p].[Id] > 1
```

Таким чином, всі методи підсумовуються, запит оптимізується, і тільки потім відбувається вибірка з бази даних.

Це дозволяє динамічно створювати складні запити. Наприклад, ми можемо послідовно нашаровувати в залежності від умов вираження для фільтрації:

```csharp
IQueryable<User> userIQuer = db.Users;
userIQuer = userIQuer.Where(p => p.Id < 7);
userIQuer = userIQuer.Where(p => p.Name == "Tom"); 
var users = userIQuer.ToList();
```

В даному випадку буде створюватися наступний SQL-запит:

```SQL
SELECT [u].[Id], [u].[Name]
FROM [Users] AS [u]
WHERE ([p].[Id] < 7) AND (([u].[Name] = N'Tom') AND [u].[Name] IS NOT NULL)
```