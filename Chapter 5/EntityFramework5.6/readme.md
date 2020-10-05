# Entity Framework Core (Chapter 5, Lesson 6)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.5.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розіді я розгялнув декілька методів LinQ які дозволяють нам працювати з даними БД, а саме:
* **Sum** - загальна сума по певному значенню, наприклад ціна
* **Min** - мінімальне значення у вибірці по певному критерію, наприклад ціна
* **Max** - максимальне значення у вибірці по певному критерію, наприклад ціна
* **Average** - середнє значення по всій вибірці по певному критерію, наприклад ціна
* **Any** - вертає true/false в залежності чи є хоча б один елемент який задовільняє певній умові
* **All** - вертає true/false у випадку, якщо всі елементи колекції задовільняють певну умову
* **Count** - вертає кількість елементів які задовільняють певну умову

### Models
В цьому розділі я працював з такими моделями:

```csharp
class Phone
{
    [Key]
    public int PhoneID { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }

    public int CompanyID { get; set; }
    public Company Company { get; set; }

    public Phone() { }
    public Phone(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException();

        this.Name = Name;
    }
    public Phone(string Name, int Price) : this(Name)
    {
        if (Price <= 0)
            throw new ArgumentException();

        this.Price = Price;
    }
    public override string ToString()
    {
        return $"Phone ID: {this.PhoneID}\n\t" +
                $"Name: {this?.Name}\n\t" +
                $"Price: {this?.Price}\n\t" +
                $"Company: {this?.Company?.Name}";
    }
}

class Company
{
    [Key]
    public int CompanyID { get; set; }
    public string Name { get; set; }

    public List<Phone> Phones { get; set; }

    public Company() { }
    public Company(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException();
        this.Name = Name;
    }
    public override string ToString()
    {
        return $"Company ID: {this.CompanyID}\n\t" +
                $"Name: {this?.Name}";
    }
}
```

### Sum
Для отримання суми значень можна використати метод **Sum()**:

```csharp
public static int GetTotalCompanyPhonePrice(string companyName)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (string.IsNullOrWhiteSpace(companyName))
            return -1;
        else
        {
            var phonesByCompany = db.Companies.Include(c => c.Phones)
                                              .FirstOrDefault(c => c.Name == companyName).Phones;

            if (phonesByCompany == null)
                return -1;
            else
                return phonesByCompany.Sum(p => p.Price);
        }    
    }
}
```
### Min
Для отримування мінімального значення з колекції за певною умовою можна використати метод **Min()**:

```csharp
public static Phone GetPhoneWithMinPrice()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        return db.Phones.Include(p => p.Company).FirstOrDefault(p => p.Price == db.Phones.Min(p => p.Price));
    }
}
```

### Max
Для отримування максимального значення з колекції за певною умовою можна використати метод **Max()**:

```csharp
public static Phone GetPhoneWithMaxPrice()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        return db.Phones.Include(p => p.Company).FirstOrDefault(p => p.Price == db.Phones.Max(p => p.Price));
    }
}
```

### Average
Для отримування середнього значення з колекції за певною умовою можна використати метод **Average()**:

```csharp
public static double AveragePriceForCompanyPhone(string companyName)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (string.IsNullOrWhiteSpace(companyName))
            return -1;
        else
            return db.Phones.Where(p => p.Company.Name == companyName)
                            .Average(p => p.Price);
    }
}
```

### Any
Для того щоб дізнатися чи в колекції є хоча б один елемент який виконує певну умову можна використати метод **Any()**:

```csharp
public static bool CheckForCompany(string companyName)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (string.IsNullOrWhiteSpace(companyName))
            return false;
        else
            return db.Companies.Any(c => c.Name == companyName);
    }
}
```

### All
Для того щоб дізнатися, чи всі елементи певної колеції виконують певну умову можна використати метод **All()**:

```csharp
public static bool CheckAllPhonesForCompany(string companyName)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (string.IsNullOrWhiteSpace(companyName))
            return false;
        else
            return db.Phones.All(p => p.Company.Name == companyName);
    }
}
```

### Count
Для того щоб дізнатися кількість елементів колекції які виконують певну умову можна використати метод **Count()**:

```csharp
public static int GetCountOfCompanyPhones(string companyName)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        if (string.IsNullOrWhiteSpace(companyName))
            return -1;
        else
            return db.Phones.Count(p => p.Company.Name == companyName);
    }
}
```