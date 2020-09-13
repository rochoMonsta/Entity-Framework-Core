using System;
using System.Linq;

namespace EFConnectToDB
{
    class Program
    {
        static void Main(string[] args)
        {
            GetUsersFromDBAndPrintIt();
            Console.ReadLine();
        }
        static void GetUsersFromDBAndPrintIt()
        {
            using (EFUsersContext db = new EFUsersContext())
            {
                var userList = db.Users.ToList();

                foreach (var user in userList)
                    Console.WriteLine($"User ID: {user.Id}\nName: {user.Name}\nSurname: {user.Surname}\nAge: {user.Age}\n");
            }
        }
    }
}
