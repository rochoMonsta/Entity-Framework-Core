# Entity Framework (Chapter 2, Lesson 3)

## Коротка інформація
[Lesson link](https://metanit.com/sharp/entityframeworkcore/2.4.php)

## Короткий конспект
Створення таблиці в БД по заданій моделі, в даному випадку *Country*:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Country>();
}
```
Ігнорування створення таблиці, в даному випадку таблиці по моделі *Company*:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Ignore<Company>();
}
```
Ігнорування за допомогою анотацій:
```csharp
[NotMapped]
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```