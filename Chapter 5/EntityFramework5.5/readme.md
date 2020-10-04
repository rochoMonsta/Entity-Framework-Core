# Entity Framework Core (Chapter 5, Lesson 5)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.4.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розділі я розглянув 3 методи групування даних за допомогою засобів LinQ:

Ряд методів Linq дозволяють працювати з результатами вибірки як з множинами, виробляючи операції на об'єднання, перетин, різниця двох вибірок.

Але перед використанням даних методів треба враховувати, що вони проводяться над однорідними вибірками з однаковим визначенням рядків, 
тобто які збігаються за складом стовпців.

* **Union()** - об'єднання
* **Intersect()**: - пересікання
* **Except()**: - виключення (разность)

### Union

За допомогою методу **Union()** ми можемо об'єднати дві колекції даних у випадку, якщо вони обидві є колекціями одного й того самого типу.
Розглянемо приклад для моделей *Phone* та *Company*:

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

    public List<Phone> Phones { get; set; }

    public Company() { Phones = new List<Phone>(); }
    public Company(string Name)
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ArgumentNullException();

        this.Name = Name; Phones = new List<Phone>();
    }
}
```

Напишемо логіку, яка буде виконувати об'єднання колекції телефонів, ціна яких перевищує 700 ум/од та тих телефонів, в назві яких є ключове слово *Xiaomi*:

```csharp
public static void phoneWithCompany()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // За допомогою метода Where отримуємо колекцію телефонів, ціна яких перевищує 700 ум/од
        var phoneWithCompany = db.Phones.Where(p => p.Price > 700)
        // За допомогою Where отримуємо колекцію телефонів, в назві яких присутє ключове слово Xiaomi
                                        // За допомогою Union виконуємо об'єднання цих двох колекцій
                                        .Union(db.Phones.Where(p => p.Name.Contains("Xiaomi")));

        // Виводимо ті телефони, ціна яких перевищує 700 ум/од та в імені яких присутє ключове слово Xiaomi
        foreach (var phone in phoneWithCompany)
            Console.WriteLine(phone);
    }
}
```

### Intersect

За допомогою метода **Intersect()** ми можемо отримати перети колекцій даних. Наприклад у нас є колекція телефонів, ціна яких перевищує 700  ум/од 
, а також колецію телефонів вироблених компанією Apple. Результатом виконання методу **Intersect()** буде колекція даних, які є як в першій колекції так і в другій.
В нашому випадку телефони компанії Apple які коштують дорожче 700  ум/од :

```csharp
public static void phoneWithCompany()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Отримуємо колекцію телефонів чия вартість перевищує 700 ум/од
        var phones = db.Phones.Where(p => p.Price > 700)
        //Отримуємо колекцію телефонів компанії Apple
                                // Отримуємо колекцію значень які є як в першій колекції так і в другій
                                .Intersect(db.Phones.Where(p => p.Company.Name == "Apple"));

        // Виводимо ті телефони компанії Apple чия вартість перевищує 700 ум/од
        foreach (var phone in phones.Include(p => p.Company))
            Console.WriteLine(phone);
    }
}
```

### Intersect

За допомогою метода **Intersect()** ми можемо отримати колекцію значень, які присутні в першій вибірці але відсутні в другій. 

Наприклад уявимо, що нам потрібно вибрати тільки ті телефони ціна яких перевищує 600 ум/од, але щоб ці телефони були створені не компанією Apple. Для 
отримання подібного результату ми можемо використати метод **Intersect()**:

```csharp
public static void phoneWithCompany()
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Отримуємо колекцію тих телефонів, чия вартість перевищує 600
        var selector1 = db.Phones.Where(p => p.Price > 600);
        // Отримуємо колекцію тих телефонів, в який ім'я компанії - "Apple"
        var selector2 = db.Phones.Where(p => p.Company.Name == "Apple");

        // Отримуємо колекцію телефонів, які присутні в першій вибірці, але відсутні в другій
        // Оскільки перша вибірка має багато телефонів компанії "Apple" та тільки 1 телефон компанії "Samsung"
        // після виконання операції ми отримаємо тільки телефон компанї "Samsung", оскільки решту телефонів компанї "Apple"
        // приустні в списку телефонів компанї "Apple"
        var phones = selector1.Except(selector2);

        // Виводимо ті телефони, чия вартість перевищує 600 ум/од та які не є створені компанією Apple
        foreach (var phone in phones.Include(p => p.Company))
            Console.WriteLine($"Phone name: {phone.Name} - Price: {phone.Price} - Company: {phone?.Company?.Name}");
    }
}
```