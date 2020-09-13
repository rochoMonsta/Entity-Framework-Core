using EntityFramework2._4.Models;
using System;
using System.Linq;

namespace EntityFramework2._4.Context
{
    class PersonTableLogic
    {
        public void AddPerson(Person person)
        {
            using (PersonContext db = new PersonContext())
            {
                if (!db.People.ToList().Any(x => x.Name == person.Name && x.Surname == person.Surname && x.Age == person.Age))
                {
                    db.People.Add(person);
                    db.SaveChanges();
                }
            }
        }
        public void PrintPeople()
        {
            using (PersonContext db = new PersonContext())
            {
                foreach (var person in db.People.ToArray())
                    Console.WriteLine(person);
            }
        }
    }
}
