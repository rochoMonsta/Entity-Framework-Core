# Entity Framework (Chapter 2, Lesson 4)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/2.5.php)

## Короткий конспект
* По замовчуванню модель включає в себе свойства які визначенні як публічні. Приватні будуть ігноруватись.
* Ми можемо використовувати як автосвойства, так і свойства з певною логікою

   ```csharp
   public class Product
   {
        private string _name;
     
        public int Id { get; set; }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        public int Price { get; set; }
   }
   ```
* Ми можемо виключати свойства по яким не потрібно створювати стовпці в таблиці:
   
   * За допомогою Fluent API (в моделі Product ігноруємо свойсво Rate):
        ```csharp
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Ignore(b => b.Rate);
        }
        ```
   * За допомогою анотацій (ігноруємо свойсво Rate за допомогою атрибуту *[NotMapped]* ):
        ```csharp
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Price { get; set; }
            [NotMapped]
            public int Rate { get; set; }
        }
        ```

