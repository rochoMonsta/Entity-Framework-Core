using System;
using System.Linq;

namespace EntityFramework
{
    sealed class DBLogic
    {
        public void AddNewElementToDB(User user)
        {
            using (UserContext db = new UserContext())
            {
                if (!db.Users.ToList().Any(us => us.Name == user.Name && us.Surname == user.Surname && us.Age == user.Age))
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
            }
        }
        public void AddNewElementToDB(params User[] users)
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in users)
                    if (!db.Users.ToList().Any(us => us.Name == user.Name && us.Surname == user.Surname && us.Age == user.Age))
                        db.Users.Add(user);
                db.SaveChanges();
            }
        }
        public void PrintElementsInDB()
        {
            using (UserContext db = new UserContext())
            {
                foreach (var element in db.Users.ToList())
                    Console.WriteLine(element);
            }
        }
        public void ChangeElementById(int id, string name = "Roman", string surname = "Cholkan", int age = 20)
        {
            using (UserContext db = new UserContext())
            {
                var elementById = db.Users.FirstOrDefault(some => some.Id == id);
                if (elementById != null)
                {
                    elementById.Name = name;
                    elementById.Surname = surname;
                    elementById.Age = age;

                    db.Users.Update(elementById);
                    db.SaveChanges();
                }
                else
                    throw new Exception($"User with this id: ({id}) not found.");
            }
        }
    }
}
