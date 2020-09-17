# Entity Framework (Chapter 3, Lesson 1)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/3.1.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

Для зв'язків між моделями в Entity Framework Core застосовуються зовнішні ключі і навігаційні властивості. Так, візьмемо наприклад такі моделі:
```csharp
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; } // Назва компанії
     
    public List<User> Users { get; set; } // Навігаційне свойство
}
 
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
 
    public int CompanyId { get; set; }      // Зовнішній ключ
    public Company Company { get; set; }    // Навігаційне свойство
}
 
public class ApplicationContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<User> Users { get; set; }
    public ApplicationContext()
    {
        Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=relationsdb;Trusted_Connection=True;");
    }
}
```
В даному випадку сутність Company є головною сутністю, а клас User - залежною, так як містить посилання на клас Company і залежить від цього класу.
Зовнішній ключ залежної моделі має називатись за декількомпа правила, одне з яких "Назва моделі" + "Назва свойства на яке треба посилатись" - CompanyId.

Це правило можна обійти за допомогою атрибутів або Fluent API:
* За допомогою атрибуту **[ForeignKey]**
    ```csharp
    using System.ComponentModel.DataAnnotations.Schema;
 
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
    
        public int CompanyInfoKey { get; set; }
        [ForeignKey("CompanyInfoKey")]
        public Company Company { get; set; }
    }
    ```
* За допомогою засобів Fluent API та методу **HasForeignKey**:
    ```csharp
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            // Вказуємо зв'язок між моделями. HasOne - може бути одна компанія за якою закріплені багато користувачів
            .HasOne(p => p.Company)
            // Вказуємо, що користувачі залежать від компанії
            .WithMany(t => t.Users)
            // Вказуємо свойство яке буде зовнішнім ключем
            .HasForeignKey(p => p.CompanyInfoKey);
    }
    ```
Крім того, за допомогою Fluent API ми можемо зв'язати зовнішній ключ не тільки з первинними ключами пов'язаних сутностей, а й з іншими властивостями. наприклад:
```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(p => p.Company)
            .WithMany(t => t.Users)
            .HasForeignKey(p => p.CompanyName)
            //Метод HasPrincipalKey вказує на властивість пов'язаної суті, на яку буде посилатися властивість-зовнішній ключ CompanyName.
            .HasPrincipalKey(t=>t.Name)
    }
```
Крім того, для властивості, зазначеного в HasPrincipalKey (), буде створювати альтернативний ключ.