# Entity Framework Core (Chapter 5, Lesson 4)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.3.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розділі я дізнався про ще один спосіб, за допомогою якого можна згрупувати дані з декількох пов'язаних таблиць, а саме метод **Join()**.
Дізнався про те, як можна згрупувати дані за певним ключем, наприклад компанією або країною.

### Join()

Для об'єднання таблиць за певним критерієм в Entity Framework Core використовується метод Join. Наприклад, 
в нашому випадку таблиця телефонів і таблиця компаній має загальний критерій - id компанії, за яким можна провести об'єднання таблиць:

```csharp
public static void PrintAllPhones()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        var phones = db.Phones.Join(db.Companies, // з'єднуємо таблицю компаній з поточною таблицею телефонів
            p => p.PhoneID, // вибираємо властивість по якій буде відбуватись об'єднання, в даному випадку ID телефону
            c => c.CompanyID, // вибираємо властивість по якій буде відбуватись об'єднання, в даному випадку ID компанії
            (p, c) => new // створюємо новий анонімний тип який об'єднує основні властивості
            {
                Name = p.Name,
                Company = c.Name,
                Price = p.Price
            });
        foreach (var phone in phones)
            Console.WriteLine($"Name: {phone.Name}\n\t" +
                              $"Price: {phone.Price}\n\t" +
                              $"Company: {phone.Company}");
    }
}
```

Метод **Join()** приймає 4 параметра:
* другу таблицю, яка з'єднується з поточною
* властивість об'єкта - стовпець з першої таблиці, за яким йде з'єднання
* властивість об'єкта - стовпець з другої таблиці, за яким йде з'єднання
* новий об'єкт, який виходить в результаті з'єднання

Аналогічний результат можна отримати наступним чином:

```csharp
var phones = from p in db.Phones
    join c in db.Companies on p.CompanyId equals c.Id
    select new { Name=p.Name, Company = c.Name, Price=p.Price };
```

### Об'єднання трьох та більще таблиць

Припустимо у нас є 3 таблиці які взаємодіють між собою (один до багатьох):

```csharp
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
                $"Company: {this?.Company?.Name}";
    }
}
class Company
{
    [Key]
    public int CompanyID { get; set; }
    public string Name { get; set; }

    public int CountryID { get; set; }
    public Country Country { get; set; }

    public List<Phone> Phones { get; set; }

    public Company() { Phones = new List<Phone>(); }
    public Company(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException();

        this.Name = Name; Phones = new List<Phone>();
    }
    public Company(string Name, Country Country) : this(Name)
    {
        this.Country = Country;
    }
}
class Country
{
    public int ID { get; set; }
    public string Name { get; set; }

    public List<Company> Companies { get; set; }
}
```

Щоб об'єднати дані з цих 3 таблиць та вивести основну інформацію, наприклад (назву телефона, його ціну, компанію яка його створила 
та місце де ця компанія розташовується) можна використати:

```csharp
public static void PrintFullInfo()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Групуємо дані з таблиць телефони, компанії, країни за основними ключами які поєднують ці таблиці
        var phones = from phone in db.Phones // Основна таблиця
                        join company in db.Companies on phone.CompanyID equals company.CompanyID // свойство селектора з першої таблиці
                        join country in db.Countries on company.CountryID equals country.ID // свойство селектора з другої таблиці
                        select new // новий анонімний тип який буде групувати всі основні елементи з всіх згруповани таблиць
                        {
                            Name = phone.Name,
                            Price = phone.Price,
                            Company = company.Name,
                            Country = country.Name
                        };
        foreach (var phone in phones)
            Console.WriteLine($"Name: {phone.Name}\n\t" +
                              $"Company: {phone.Company}\n\t" +
                              $"Company location: {phone.Country}\n\t" +
                              $"Price: {phone.Price}");
    }
}
```

### Групування за допомогою GroupBy

Для групування даних за певними критеріями застосовується оператор **group by**, або метод **GroupBy()**. Наприклад, згрупуємо моделі смартфонів по виробнику:

```csharp
public static void GroupByCompany()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Групуємо телефони за назвою компанії яка їх створила
        var group = from phone in db.Phones
                    group phone by phone.Company.Name into dict
                    select new
                    {
                        dict.Key, // Ключем в даному випадку виступає назва компінй
                        Count = dict.Count() // Count - кількість телефонів які створила певна компанія
                    };
        foreach (var company in group)
            Console.WriteLine($"Company {company.Key} - Phones {company.Count}");
    }
```

Аналогічний результат можна отримати за допомогою метода **GroupBy()** LinQ:

```csharp
public static void GroupByCompany()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Same to first example
        var companyPhonesCount = db.Phones.GroupBy(p => p.Company.Name)
                                          .Select(g => new
                                          {
                                              g.Key,
                                              Count = g.Count()
                                          });
        foreach (var company in companyPhonesCount)
            Console.WriteLine($"Company {company.Key} - Phones {company.Count}");
    }
```