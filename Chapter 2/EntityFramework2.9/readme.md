# Entity Framework Core (Chapter 2, Lesson 9)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.10.php)

## Короткий конспект

### Типи даних

* int : int
* bit : bool
* char : string
* date : DateTime
* datetime : DateTime
* datetime2 : DateTime
* decimal : decimal
* float : double
* money : decimal
* nchar : string
* ntext : string
* numeric : decimal
* nvarchar : string
* real : float
* smallint : short
* text : string
* tinyint : byte
* varchar : string

По стандарту EF для свойства з типом string буде використовуватись nvarchar.

### Анотації даних
За допомогою атрибуту **Column** можна задати тип для свойства в БД:
```csharp
public class User
{
    public int Id { get; set; }
    [Column(TypeName = "varchar(200)")]
    public string Name { get; set; }
}
```
Те ж саме можна виконати за допомогою Fluent API:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>().Property(u=>u.Name).HasColumnType("varchar(200)");
}
```
