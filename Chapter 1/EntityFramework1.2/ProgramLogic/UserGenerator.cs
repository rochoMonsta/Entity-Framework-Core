using System;
using System.Linq;

namespace EF.ProgramLogic
{
    static class UserGenerator
    {
        private static Random random = new Random(Guid.NewGuid().ToByteArray().Sum(x => x));

        private static string[] Names =
        {
            "Roman", "Sasha", "Igor", "Anton", "Vadim", "Mia", "Olha", "Dana", 
            "Christina", "Vika", "Yaryna", "Lesya", "Nataliya", "Nastya"
        };
        private static string[] Surnames =
        {
            "Cholkan", "Brooks", "Connor", "Stone", "Silver", "Gold", "Smith", 
            "Jakson", "Wills", "Jonson", "Hawking", "Bakwel", "Wood", "Anderson"
        };
        public static User CreateNewUser(this User user)
        {
            user.Name = Names[random.Next(Names.Length)];
            user.Surname = Surnames[random.Next(Surnames.Length)];
            user.Age = random.Next(1, 110);

            return user;
        }
    }
}
