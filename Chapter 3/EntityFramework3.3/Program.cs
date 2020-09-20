using EntityFramework3._3.Context;
using EntityFramework3._3.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace EntityFramework3._3
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                #region Data initialization
                Position manager = new Position("Manager");
                Position developer = new Position("Developer");
                db.Positions.AddRange(manager, developer);

                City washington = new City("Washington");
                db.Cities.Add(washington);

                Country usa = new Country("USA", washington);
                db.Countries.Add(usa);

                Company google = new Company("Google", usa);
                Company apple = new Company("Apple", usa);
                db.Companies.AddRange(google, apple);

                db.SaveChanges();

                User[] users =
                {
                    new User("Roman Cholkan", apple, developer),
                    new User("Mia Sorokotyaha", apple, manager),
                    new User("Vadim Yakovlev", google, manager),
                    new User("Orest Chubak", google, developer)
                };
                db.Users.AddRange(users);

                db.SaveChanges();
                #endregion

                #region Eager loading
                //var usersFromDB = db.Users.Include(u => u.Company)
                //                            .ThenInclude(c => c.Country)
                //                            .ThenInclude(c => c.Capital)
                //                          .Include(u => u.Position)
                //                    .ToList();

                //foreach (var user in usersFromDB)
                //{
                //    Console.WriteLine($"{user.Name} - {user.Position.Name}");
                //    Console.WriteLine($"{user.Company?.Name} - {user.Company?.Country?.Name} - {user.Company?.Country.Capital.Name}");
                //    Console.WriteLine("---------------------------------");
                //}
                #endregion

                #region Explicit loading
                Company company = db.Companies.FirstOrDefault();
                db.Users.Where(p => p.CompanyID == company.CompanyID).Load();

                Console.WriteLine($"Company: {company.Name}");
                foreach (var p in company.Users)
                    Console.WriteLine($"User: {p.Name}; Position: {p.Position?.Name}; {p.Company?.Country?.Name}; " +
                                      $"{p.Company?.Country?.Capital?.Name}");

                #endregion

                #region Lazy loading
                // Все навигационные свойства должны быть определены как виртуальные (то есть с модификатором virtual), 
                // при этом сами классы моделей должны быть открыты для наследования

                //var usersLazy = db.Users.ToList();
                //foreach (var user in usersLazy)
                //    Console.WriteLine($"{user.Name} - " +
                //                      $"{user.Company?.Name} - " +
                //                      $"{user.Company?.Country?.Name} - " +
                //                      $"{user.Company?.Country?.Capital?.Name} - " +
                //                      $"{user.Position?.Name}");

                //var company = db.Companies.FirstOrDefault();
                //foreach (var user in company.Users)
                //    Console.WriteLine(user);
                #endregion


                Console.ReadLine();
            }
        }
    }
}
