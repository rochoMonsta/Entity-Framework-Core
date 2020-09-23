# Entity Framework Core (Chapter 2, Lesson 7)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.8.php)

## Короткий конспект

По стандарту значення наших свойств генеруються автоматично за допомогою БД в тому випадку, якщо вони не встановлені 
(null для string, 0 для int, Guid.Empty для Guid і т.д.).

Для первиного ключа значення генерується завжди автоматично. Проте цю логіку можна вимкнути. Тоді значення прийдеться задавати вручну.

### Відключення автогенерації значення

* За допомогою атрибуту:

    Атрибут **DatabaseGeneratedAttribute** представляє анотацію, яка дозволяє змінити поведінку БД при додавані або змінені.
    Наприклад ми хочемо вимкнути автогенерацію ключів:
    ```csharp
    using System.ComponentModel.DataAnnotations.Schema;
 
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    ```
    Якщо тепер спробувати додати елементи в БД, то при додавані 2 і більше виникне помилка, тому що значення Id будуть рівні нулю.
    Щоб уникнути подібної помилки, потрібно вручну задавати значення Id та так, щоб вони не повторювались:
    ```csharp
    using (ApplicationContext db = new ApplicationContext())
    {
        db.Users.Add(new User { Id = 1, Name = "Tom" });
        db.Users.Add(new User { Id = 2, Name = "Alice" });
        db.SaveChanges();
        var users = db.Users.ToList();
        foreach (var user in users) 
            Console.WriteLine($"{user.Id} - {user.Name}");
    }
    ```
* За допомогою засобів Fluent API

    Відключення автогенерації значень за допомогою Fluent API
    ```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.Id).ValueGeneratedNever();
    }
    ```

### Автогенерація значень

* За допомогою атрибуту **[DatabaseGeneratedOption.Identity]**:
    ```csharp
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
    ```
* За допомогою засобів Fluent API

    Ми можемо задати те значення яке буде приймати певне свойство в тому випадку, якщо воно не задано:
    ```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Age == 18 в тому випадку, якщо Age - не задано
        modelBuilder.Entity<User>().Property(u => u.Age).HasDefaultValue(18);
    }
    ```
### HasDefaultValueSql

Метод **HasDefaultValueSql** також виконує автогенерацію значення, проте на основі SQL коду який передається в цей метод.

Наприклад нехай в класі буде свойство **public DateTime CreatedAt { get; set; }** яке відповідає за дату додавання користувача в БД.
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime CreatedAt { get; set; }
}
```
Для генерації значення цього свойства використовуємо функцію GETDATE();
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .Property(u => u.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
    }
```
У метод HasDefaultValueSql () передається SQL-вираз, який викликається при додаванні об'єкта User в базу даних.

### Обчислювані стовпці

Ми можемо автоматично генерувати значення певного свойства на основі значень з інших свойств за допомогою Fluent API:
```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get;}
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }
}
```
Нам потрібно автоматично згенерувати значення **Name** на основі поєднання **FirstName** + **[ ]** + **LastName**
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .Property(u => u.Name)
                    .HasComputedColumnSql("[FirstName] + ' ' + [LastName]");
    }
```
