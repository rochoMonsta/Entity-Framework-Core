using System;
using System.Linq;

namespace EFMigration
{
    class Program
    {
        static void Main(string[] args)
        {
            //AddUser(new User("Roman", "Cholkan", 20));
            //AddUser(new User("Mia", "Sorokotyaha", 20));
            ChangeUserById(1);

            GetUsers();
            Console.ReadLine();
        }
        public static void AddUser(User user)
        {
            using (UserContext db = new UserContext())
            {
                if (!db.Users.ToList().Any(us => us.Name == user.Name && us.Surname == user.Surname))
                    db.Users.Add(user);
                db.SaveChanges();
            }
        }
        public static void GetUsers()
        {
            using (UserContext db = new UserContext())
            {
                foreach (var user in db.Users.ToList())
                    Console.WriteLine(user);
            }
        }
        public static void ChangeUserById(int id)
        {
            using (UserContext db = new UserContext())
            {
                var currentUser = db.Users.ToList().FirstOrDefault(us => us.Id == id);
                if (currentUser != null)
                {
                    currentUser.IsMarried = true;
                    db.Users.Update(currentUser);
                    db.SaveChanges();
                }
                else
                    throw new Exception($"User with this id: ({id}) not found.");
            }
        }
    }
}
