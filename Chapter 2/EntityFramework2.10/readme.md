# Entity Framework (Chapter 2, Lesson 10)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.13.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В версії EF 2.0 була добавлена можливість виносити конфігурації моделей та свойств в окремі класи. Навіщо це? За допомогою Fluent API
ми можемо задати налаштування для моделей та свойств, і часто буває момент, коли цих налаштувань є досить багато. І щоб не перегружати
клас контексту, ми можемо винести налаштування кожної моделі в окремий клас який буде реалізовувати інтерфейс **EntityTypeConfiguration<T>**.

Наприклад у нас є наступний клас контексту та моделей:
```csharp
public class ApplicationContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Product> Products { get; set; }
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=mobileappdb;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
                .ToTable("Mobiles").HasKey(p => p.Ident);
        modelBuilder.Entity<Product>()
                .Property(p => p.Name).IsRequired().HasMaxLength(30);
 
        modelBuilder.Entity<Company>().ToTable("Manufacturers")
                .Property(c => c.Name).IsRequired().HasMaxLength(30);
    }
}
 
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
 
    public List<Product> Products { get; set; }
    public Company()
    {
        Products = new List<Product>();
    }
}
 
public class Product
{
    public int Ident { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
}
```
Вся конфігурація тут визначена в методі **OnModelCreating()**. І в принципі, наразі тут мало налаштувань, але якщо їх буде більше, 
то цей метод може стати надто громістким, для цього, розіб'ємо налаштування окремих моделей на окермі класи:
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
 
public class ApplicationContext : DbContext
{
    public DbSet<Company> Companies { get; set; }
    public DbSet<Product> Products { get; set; }
    public ApplicationContext()
    {
        Database.EnsureCreated();
    }   
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=mobileappdb2;Trusted_Connection=True;");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CompanyConfiguration());
    }
}
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Mobiles").HasKey(p => p.Ident);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
    }
}
public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
    public void Configure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Manufacturers").Property(c => c.Name).IsRequired().HasMaxLength(30);
    }
}
```
Тепер конфігурація моделей винесена в окремі класи, екзепляри який ми викликаємо в методі **OnModelCreating()**.

В якості альтернативи, ми могли б створити окремі методи для налаштування конфігурацій прямо в класі контексту:
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
 
public class ApplicationContext : DbContext
{
    ...
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(ProductConfigure);
        modelBuilder.Entity<Company>(CompanyConfigure);
    }
    // конфігурація для Product
    public void ProductConfigure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Mobiles").HasKey(p => p.Ident);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(30);
    }
    // конфігурація для Company
    public void CompanyConfigure(EntityTypeBuilder<Company> builder)
    {
        builder.ToTable("Manufacturers").Property(c => c.Name).IsRequired().HasMaxLength(30);
    }
}
```