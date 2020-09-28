# Entity Framework Core (Chapter 5, Lesson 1)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.1.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

Для виконання певної вибірки з БД використовуються засоби LinQ to Entitys. За допомогою засобів LinQ ми можемо працювати з елементами БД.

Розглянемо наступний приклад (використаємо моделі зі зв'язком один до багатьох):
```csharp
class Company
{
    public int CompanyID { get; set; }
    public string Name { get; set; }
    public List<Phone> Phones { get; set; }

    public Company() { Phones = new List<Phone>(); }
    public Company(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException();

        this.Name = Name; Phones = new List<Phone>();
    }
}
class Phone
{
    [Key]
    public int PhoneID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public int CompanyID { get; set; }
    public Company Company { get; set; }

    public override string ToString()
    {
        return $"Phone ID: {this.PhoneID}\n\t" +
                $"Name: {this.Name}\n\t" +
                $"Price: {this.Price}\n\t" +
                $"Company name: {this.Company?.Name}";
    }
}
```
Та клас контексту:
```csharp
class ApplicationContext : DbContext
{
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Company> Companies { get; set; }

    public ApplicationContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF5.1;Trusted_Connection=True;");
    }
}
```
За допомогою засобів LinQ ми можемо виконувати перевірку даних на їхню наявність в БД щоб не заносити повторні значення:
```csharp
public static void AddNewPhone(Phone phone)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        //Виконуємо перевірку чи є в БД телефон з назвою телефону, який ми хочемо додати
        if (!db.Phones.Any(p => p.Name == phone.Name))
        {
            //Виконуємо чи в таблиці компаній вже є компанія з назвою компанії нашого телефону, якщо є, то просто присвоюємо індекс цієї коипанії
            if (db.Companies.Any(c => c.Name == phone.Company.Name))
            {
                //Вибираємо перше значення яке відповідає нашій умові, якщо такого значення не існує, то присвоється стандартне значення
                phone.CompanyID = db.Companies.FirstOrDefault(c => c.Name == phone.Company.Name).CompanyID;
                phone.Company = null;
            }
            db.Phones.Add(phone);
            db.SaveChanges();
        }
    }
}
```
Ми також можемо вибрати тільки ті елементи, які відповідають певній умові, за допомогою ключового слова **Where**:
```csharp
public static void PrintPhonesByCompanyID(int companyID)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        //var phoneCompaniesCollection = (from phone in db.Phones.Include(p => p.Company)
        //                                where phone.CompanyID == companyID
        //                                select phone).ToList();

         //LinQ (Where) - відбирає ті елементи які задовільняють умові (вибірка з багатьох по умові)
        var phoneCompaniesCollection = db.Phones.Include(p => p.Company)
                                                .Where(p => p.CompanyID == companyID);
        foreach (var phone in phoneCompaniesCollection)
            Console.WriteLine(phone);
    }
}
```
## Загальні методи LinQ
* **All / AllAsync**: - повертає true, якщо всі елементи набору задовольняють певній умові, інакше повертає false
* **Any / AnyAsync**: - повертає true, якщо хоча б один елемент набору задовільняє певній умові
* **Average / AverageAsync**: - підраховує cреднее значення числових значень в наборі
* **Contains / ContainsAsync**: - визначає, чи містить набір певний елемент
* **Count / CountAsync**: - підраховує кількість елементів в наборі по заданій умові
* **First / FirstAsync**: - вибирає перший елемент колекції
* **FirstOrDefault / FirstOrDefaultAsync**: - вибирає перший елемент колекції або повертає значення за замовчуванням
* **Single / SingleAsync**: - вибирає єдиний елемент колекції, якщо колекція містить більше або менше одного елемента, то генерується виняток
* **SingleOrDefault / SingleOrDefaultAsync**: - вибирає перший елемент колекції або повертає значення за замовчуванням
* **Select:** - визначає проекцію обраних значень
* **Where:** - визначає фільтр вибірки
* **OrderBy:** - впорядковує елементи по зростанню
* **OrderByDescending:** - впорядковує елементи за спаданням
* **ThenBy:** - задає додаткові критерії для упорядкування елементів зростанню
* **ThenByDescending:** - задає додаткові критерії для упорядкування елементів за спаданням
* **Join:** - з'єднує два набори за певною ознакою
* **GroupBy:** - групує елементи по ключу
* **Except:** - повертає різницю двох наборів, тобто ті елементи, які містяться тільки в одному наборі
* **Union:** - об'єднує два однорідних набори
* **Intersect:** - повертає перетин двох наборів, тобто ті елементи, які зустрічаються в обох наборах елементів
* **Sum / SumAsync:** - підраховує суму числових значень в колекції
* **Min / MinAsync:** - знаходить мінімальне значення
* **Max / MaxAsync:** - знаходить максимальне значення
* **Take:** - вибирає певну кількість елементів з початку послідовності
* **TakeLast:** - вибирає певну кількість елементів з кінця послідовності
* **Skip:** - пропускає певну кількість елементів з початку послідовності
* **SkipLast:** - пропускає певну кількість елементів з кінця послідовності
* **TakeWhile:** - повертає ланцюжок елементів послідовності, до тих пір, поки умова істинна
* **SkipWhile:** - пропускає елементи в послідовності, поки вони задовольняють заданій умові, і потім повертає елементи які залишились
* **ToList / ToListAsync:** - отримання списку об'єктів

Для більшості методів визначено асинхронні версії, при необхідності отримувати дані в асинхронному режимі, ми можемо їх задіяти.