using EntityFramework2._4.Context;
using EntityFramework2._4.Models;
using System;

namespace EntityFramework2._4
{
    class Program
    {
        public static PersonTableLogic pLogic = new PersonTableLogic();
        static void Main(string[] args)
        {
            Person person1 = new Person("Roman", "Cholkan", 20);
            Person person2 = new Person("Mia", "Sorokotyaha", 20);

            pLogic.AddPerson(person1);
            pLogic.AddPerson(person2);

            pLogic.PrintPeople();

            Console.ReadLine();
        }
    }
}
