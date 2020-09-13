using EF.ProgramLogic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace EF
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Get server options
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;
            #endregion

            GetAndPrintUsersFromDB(options);

            Console.ReadLine();
        }
        /// <summary>
        /// Crete random user and add it to DB
        /// </summary>
        /// <param name="countOfUsers" - the number of users to create></param>
        /// <param name="options" - server DB options></param>
        static void CreateUsersAndAddItToDB(int countOfUsers, DbContextOptions<ApplicationContext> options)
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                for (int i = 0; i < countOfUsers; ++i)
                    db.Users.Add(UserGenerator.CreateNewUser(new User()));
                db.SaveChanges();

                Console.WriteLine("Create users and add it to db");
            }
        }
        /// <summary>
        /// Retrieving and withdrawing users from DB
        /// </summary>
        /// <param name="options" - server DB options></param>
        static void GetAndPrintUsersFromDB(DbContextOptions<ApplicationContext> options)
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var userList = db.Users.ToList();

                foreach (var user in userList)
                    Console.WriteLine($"ID:\t{user.Id}\nName:\t{user.Name}\nSurname:\t{user.Surname}\nAge:\t{user.Age}\n");
            }
        }
        /// <summary>
        /// Editing user information by its index
        /// </summary>
        /// <param name="userID" - user ID in DB></param>
        /// <param name="newName" - new name></param>
        /// <param name="newSurname" - new surname></param>
        /// <param name="newAge" - new age></param>
        /// <param name="options" - server DB options></param>
        static void ChangeUserInfoByID(int userID, string newName, string newSurname, int newAge, DbContextOptions<ApplicationContext> options)
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                User user = db.Users.FirstOrDefault(user => user.Id == userID);
                if (user != null)
                {
                    user.Name = newName;
                    user.Surname = newSurname;
                    user.Age = newAge;

                    db.Users.Update(user);
                    db.SaveChanges();
                }
                else
                    Console.WriteLine("User with this id doesn't found");
            }
        }
        /// <summary>
        /// Clear the table from users (id counter saves)
        /// </summary>
        /// <param name="options" - server DB options></param>
        static void ClearDB(DbContextOptions<ApplicationContext> options)
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                foreach (var user in db.Users.ToList())
                {
                    if (user != null)
                        db.Users.Remove(user);
                }
                Console.WriteLine("DB clear!");
                db.SaveChanges();
            }
        }
    }
}
