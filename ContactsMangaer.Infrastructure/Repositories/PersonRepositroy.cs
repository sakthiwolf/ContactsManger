using Enities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RepositroyContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class PersonRepositroy : IPersonRepositroy
    {
        private readonly PersonsDbContext _db;
        public PersonRepositroy(PersonsDbContext db)
        {
            _db = db;
        }
        public async Task<Person> AddPerson(Person person)
        {
            _db.Persons.Add(person);
           await _db.SaveChangesAsync();

            return person;

        }

        public async Task<bool> DeletePerson(Guid personId)
        {
            _db.Persons.RemoveRange(_db.Persons.Where(temp =>
              temp.PersonID == personId));
            int rowsDeleted=  await _db.SaveChangesAsync();
            return rowsDeleted >0;
        }

        public async Task<List<Person>> GetAllPersons()
        {
         return await  _db.Persons.Include("Country").ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate)
        {
            return await _db.Persons.Include("Country")
                .Where(predicate)
                .ToListAsync();
        }


        public async Task<Person?> GetPersonByPersonID(Guid personId)
        {
            return await _db.Persons.Include("Country")
               .FirstOrDefaultAsync(temp => temp.PersonID == personId);
              
        }

        public async Task<Person> UpdatePerson(Person person)
        {
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(temp =>
            temp.PersonID == person.PersonID);

            if(matchingPerson == null) 
                return person;

            matchingPerson.PersonName = person.PersonName;
            matchingPerson.Gender = person.Gender;
            matchingPerson.Address = person.Address;
            matchingPerson.Email = person.Email;
            matchingPerson.DateOfBirth = person.DateOfBirth;
            matchingPerson.CountryID = person.CountryID;
            matchingPerson.ReceiveNewsLetters = person.ReceiveNewsLetters;

            int countupdated=   await _db.SaveChangesAsync();

            return matchingPerson;

        }
    }
}
