# Entity Framework Core (Chapter 2, Lesson 6)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.7.php)

## Короткий конспект

### Первиний ключ
В даному уроці я розлянув способи якими можна задати первиний ключ, оскільки по стандарту свойства з Id або ("назва моделі"Id) будуть братись як первиний ключ.

Ми можемо задати первиний ключ за допомогою атрибуту **[Key]**
```csharp
class User
{
    [Key]
    public int UserNumber { get; set; }
    ...
}
```
Aбо за допомогою засобів Fluent API:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<User>().HasKey(u => u.UserNumber).HasName("UserId"); //HasKey - заміна [Key] через Fluent API. HasName - альтернативна назва ключа
}
```

### Составний ключ
Також я дізнався, що можна створювати составний ключ у випадку, якщо первиний ключ складається з 2 чи більше свойств. Це можливо тільки за допомогою засобів Fluent API:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    //Створення составного ключа за допомогою Fluent API та задання альтернативної назив.
    modelBuilder.Entity<User>().HasKey(u => new { u.PassportSeria, u.PassportNumber, u.UserNumber }).HasName("UserId");
}
```
### Алтернативні ключі
Ми можемо створити альтернативні ключі або составні алтернативні ключі, які як і первиний ключ мають мати унікальне значення, але в той же час вони не є первиними ключами.
Створення альтернативного ключа можливе тільки за допомогою засобів Fluent API:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    //Задаємо альтернативний ключ (Значення цього ключа також має бути унікальним, але це не первинний ключ).
    modelBuilder.Entity<User>().HasAlternateKey(u => u.PassportNumber);

    //Створення составного алтернативного ключа
    modelBuilder.Entity<User>().HasAlternateKey(u => new { u.PhoneNumber, u.PassportNumber });
}
```
### Індекcи
Засоби Fluent API дозволяють нам створити індекси які пришвидшують пошук по БД:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    //Створення індексу за допомогою HasIndex та вказування, що цей індекс є унікальним IsUnique (не повторюється)
    modelBuilder.Entity<User>().HasIndex(u => u.PassportNumber).IsUnique();

    //Створення індексів для декількох свойств.
    modelBuilder.Entity<User>().HasIndex(u => new { u.PhoneNumber, u.PassportNumber }).IsUnique();
}
```

## Результат
![alt text](https://github.com/rochoMonsta/Entity-Framework-Core/blob/master/Chapter%202/EntityFramework2.6/EntityFramework2.6(result).PNG)
