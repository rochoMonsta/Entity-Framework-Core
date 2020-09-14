# Entity Framework (Chapter 2, Lesson 5)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.6.php)

## Короткий конспект
* По замовчуванню моделі та таблиці співставляються по іменах. Це співставлення можна перевизначити за допомогою **Fluent API** та за допомогою атрибуту **[Table("бажана назва")]**
   * За допомогою **Fluent APi**
        ```csharp
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("People");
        }
        ```
   * За допомогою атрибуту **[Table("бажана назва")]**
        ```csharp
        [Table("People")]
        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
        ```
* По замовчуванню кожен стовпець в таблиці співставляється з відповідним свойством в моделі по імені. Це співставлення можна перевизначити за допомогою **Fluent API**
та за допомогою атрибуту **[Column("бажана назва")]**
   * За допомогою **Fluent APi** та метода *HasColumnName*:
        ```csharp
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(u=>u.Id).HasColumnName("user_id");
        }
        ```
   * За допомогою атрибуту **[Column("бажана назва")]**
        ```csharp
        public class User
        {       
            [Column("user_id")]
            public int Id { get; set; }
            public string Name { get; set; }
        }
        ```
## Результат
![alt text](https://github.com/rochoMonsta/Entity-Framework-Core/blob/master/Chapter%202/EntityFramework2.5/EntityFramework2.5(result).PNG)
