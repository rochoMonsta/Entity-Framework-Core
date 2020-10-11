using EntityFramework5._9.Context;
using EntityFramework5._9.Models;
using System.Collections.Generic;
using System.Linq;

namespace EntityFramework5._9
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                // IEnumerable
                int id = 1;
                IEnumerable<User> userIEnum = db.Users;
                var usersIEnum = userIEnum.Where(u => u.UserID > id).ToList();
                // ----------------------------------
                // |    SELECT [u].[Id], [u].[Name] |
                // |    FROM[Users] AS[u] |         |
                // ----------------------------------

                // IQueryable
                IQueryable<User> userIQuer = db.Users;
                var usersIQuer = userIQuer.Where(p => p.UserID > id).ToList();
                // ----------------------------------
                // |    SELECT [u].[Id], [u].[Name] |
                // |    FROM[Users] AS[u]           |
                // |    WHERE[p].[Id] > 1           |
                // ----------------------------------
            }
        }
    }
}
