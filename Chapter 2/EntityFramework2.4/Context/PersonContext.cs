using EntityFramework2._4.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EntityFramework2._4.Context
{
    class PersonContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public PersonContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EF2.4;Trusted_Connection=True;");
        }
    }
}
