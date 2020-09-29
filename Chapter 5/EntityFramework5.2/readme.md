# Entity Framework Core (Chapter 5, Lesson 2)

## Коротка інформація
[Lesson link (Metanit)](https://metanit.com/sharp/entityframeworkcore/5.2.php)

Підключені бібліотеки:

* **Microsoft.EntityFrameworkCore.Tools**
* **Microsoft.EntityFrameworkCore.SqlServer**

## Короткий конспект

В цьому розділі я розібрався з такими методами вибірки та фільтрації як:
* **Where**
* **Like**
* **Find**
* **First**
* **FirstOrDefault**

### Where

Якщо потрібно відфільтрувати дані з моделі, можна використовувати *Where*. Припустимо у нас є моделі телефонів та компаній, які їх створили, і нам 
потрібно відібрати телефони певних компаній по імені компанії. Це можна виконати наступним чином:

```csharp
public static void PrintCompanyPhones(string companyName)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Виконуємо перевірку, чи компанія з вказаною назвою існує в нашій таблиці компаній
        if (db.Companies.Any(c => c.Name == companyName))
        {
            // Якщо така компанія існує, то вибираємо з таблиці телефонів тільки ті, які були створені цією компанією
            var collection = db.Phones.Include(p => p.Company).Where(p => p.Company.Name == companyName);

            foreach (var phone in collection)
                Console.WriteLine(phone);
        }
    }
}
```

### Like

Починаючи з версії 2.0 в Entity Framework Core можна використовувати метод **EF.Functions.Like()**. 
Він дозволяє транслювати умову в вираз з оператором LIKE (по суті **Include or Contains C#**) на стороні MS SQL Server. Метод приймає два параметри - 
вираз що буде оцінюватись і шаблон, з яким порівнюється його значення. Наприклад, знайдемо всі телефони, в назві яких є слово *"Galaxy"*:

```csharp
// Виводимо ті телефони, які мають необмежену кількість символів перед словом Galaxy та після нього
PrintAllPhonesByStartedWith("%Galaxy%");

// Виводимо ті телефони, які починаються на iPhone, потім мають необмежену кількість символів, або взагалі їх не мають, після чого
// можуть містити елементи з діапазону від 6-8 після чого, знову необмежену кількість символів
PrintAllPhonesByStartedWith("iPhone%[6-8]%");
...
public static void PrintAllPhonesByStartedWith(string name)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Виконуємо перевірку, чи в таблиці телефонів є хоча б один телефон, який виконує умову name - 
        if (db.Phones.Any(p => EF.Functions.Like(p.Name, name)))
        {
            // Вибираємо ті телефони, які задовільняють умову name
            var collections = db.Phones.Include(p => p.Company).Where(p => EF.Functions.Like(p.Name, name));

            foreach (var phone in collections)
                Console.WriteLine(phone);
        }
    } 
}
```
**Для визначення шаблону можуть застосовуватися ряд спеціальних символів підстановки:**

* **%** - відповідає будь-якій підстроці, яка може мати будь-яку кількість символів, при цьому підстрока може і не містити жодного символу
* **_** - відповідає будь-якому одиночному символу
* **[ ]** - відповідає одному символу, який вказаний в квадратних дужках
* **[ - ]** - відповідає одному символу з певного діапазону
* **[ ^ ]** - відповідає одному символу, який не вказано після символу ^

### Find

Для вибірки одного елементу за його індексом ми можемо використати **Find**:

```csharp
public static Phone PhoneByID(int id)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Виконуємо перевірку, чи інсує елемент з переданим індексом
        if (db.Phones.Any(p => p.PhoneID == id))
        // Виконуємо пошук елементу на переданому індексі, та вертаємо його
            return db.Phones.Find(id);
        else
            return null;
    }
}
```

### First

Замість того, щоб використовувати метод **Find()** який є методом Entity Framework Core, ми можемо використати метод **First()** 
який поверне нам перше значення яке відповідає певній умові. Якщо такий елемент не буде знайдений, виникне помилка:

```csharp
 public static Phone PhoneByIDFirstOrDefautl(int id)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Викличе помилку якщо елемент який відповідає заданій умові не буде знайдений
        return phone = db.Phones.Include(p => p.Company).First(p => p.PhoneID == id);
    }
}
```

### FirstOrDefault

Метод **FirstOrDefault()** відрізняється від методу **First()** тим, що якщо метод **First()** не знаходить елементу, який відповідає певній умові, то він викличе помилку, в 
той час як метод **FirstOrDefault()** просто верне стандартне значення для того типу, який перераховувався:

```csharp
public static Phone PhoneByIDFirstOrDefautl(int id)
{
    using (ApplicationContext db = new ApplicationContext())
    {
        // Верне стандартне значення для типу, елементи якого перебирались, якщо умова не виконається для жодного елементу
        var phone = db.Phones.Include(p => p.Company).FirstOrDefault(p => p.PhoneID == id);

        return phone == null ? null : phone;
    }
}
```