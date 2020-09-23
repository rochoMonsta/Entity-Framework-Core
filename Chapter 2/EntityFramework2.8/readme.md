# Entity Framework Core (Chapter 2, Lesson 8)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.8.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

По замовчуванню свойства є не обов'язковими до ініціалізації якщо тип свойства допускає *null*. (string, int?, byte[], Objects)

Свойство є обов'язковим, якщо тип свойства не допускає null: (int, decimal, bool).

### Задання обов'язкової ініціалізації свойств

Ми можемо вказати, що свойство обов'язково має ініціалізовуватись. Якщо ми спробуємо передати в БД об'єкт з не ініціалізованим **обов'язковим**
свойством, то виникне помилка.

* За допомогою атрибуту **[Required]**:
    ```csharp
    public class User
    {
        public int Id { get; set; }
        [Required] //Вказує, що Name є NOT NULL
        public string Name { get; set; }
    }
    ```
* За допомогою Fluent API та методу **IsRequired()**:
    ```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.Name).IsRequired();
    }
    ```
### Обмеження по довжині

По замовчуванню строкові свойства в таблиці виглядають наступним чином: **nvarchar(MAX)**, тобто максимальна допустима довжина.
Вийнятком є випадок, коли строкове свойство є ключем, тоді максимальна допустима довжина буде складати 450 символів.

Ми можемо задани конкретну довжину для строкових свойств.

* За допомогою атрибуту **[MaxLength(довжина)]**:
    ```csharp
    public class User
    {
        public int Id { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
    }
    ```
    Також треба відмітити, що є атрибут **[MinLength]**, але він не враховується при створені стовпця.
* За допомогою Fluent API та методу **HasMaxLength()**:
    ```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(b => b.Name).HasMaxLength(50);
    }
    ```
