using System;

namespace EntityFramework
{
    class Program
    {
        public static DBLogic DBLogic = new DBLogic();
        static void Main(string[] args)
        {
            var user1 = new User()
            {
                Name = "Anton", Surname = "Lomov", Age = 20
            };
            var user2 = new User()
            {
                Name = "Mia", Surname = "Sorokotyaha", Age = 20
            };
            var user3 = new User()
            {
                Name = "Roman", Surname = "Cholkan", Age = 20
            };

            //DBLogic.AddNewElementToDB(user3);
            DBLogic.PrintElementsInDB();

            Console.ReadLine();
        }
    }
}
