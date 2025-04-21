using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Enities
{
    public class PersonsDbContext:DbContext
    {
        //options supply in program.cs dbservice
        public PersonsDbContext(DbContextOptions options):base(options) { }
       
        public DbSet<Person> Persons { get; set; }

        public DbSet<Country> Countries { get; set; }


        //configer the table name override a OnModelCreating method
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().ToTable("Countries");
            modelBuilder.Entity<Person>().ToTable("Persons");

            // Seed to Countries
            string countriesJson = System.IO.File.ReadAllText("countries.json");
            List<Country>? countries = System.Text.Json.JsonSerializer.Deserialize<List<Country>>(countriesJson);
            if (countries != null)
            {
                foreach (Country country in countries)
                {
                    modelBuilder.Entity<Country>().HasData(country);
                }
            }

            // Seed to Persons
            string personsJson = System.IO.File.ReadAllText("persons.json");
            List<Person>? persons = System.Text.Json.JsonSerializer.Deserialize<List<Person>>(personsJson);
            if (persons != null)
            {
                foreach (Person person in persons)
                {
                    modelBuilder.Entity<Person>().HasData(person);
                }
            }
        }

        //to acess storeProdcure
        public List<Person>  sp_GetAllPersons()
        {
          return  Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
        }


    }

}
