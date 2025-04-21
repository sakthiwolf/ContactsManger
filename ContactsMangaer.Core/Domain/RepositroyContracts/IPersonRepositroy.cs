using Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositroyContracts
{

    /// <summary>  
    /// Represents data access logic for managing person entities.  
    /// </summary>  
    public interface IPersonRepositroy
    {
        /// <summary>  
        /// Adds a new person object to the data store.  
        /// </summary>  
        /// <param name="person">Person object to add</param>  
        /// <returns>Returns the person object after adding it to the data store</returns>  
        Task<Person> AddPerson(Person person);

        /// <summary>  
        /// Returns all persons in the data store  
        /// </summary>  
        /// <returns>List of person object from table</returns>  
        Task<List<Person>> GetAllPersons();

        /// <summary>  
        /// Returns a person object based on the given person id  
        /// </summary>  
        /// <param name="personId">PersonID(guid) to search</param>  
        /// <returns>A person object or null </returns>  
        Task<Person?> GetPersonByPersonID(Guid personId);

        /// <summary>  
        /// Returns all person objects based on the given expression  
        /// </summary>  
        /// <param name="predicate">Filter expression</param>  
        /// <returns>List of filtered person objects</returns>  
        Task<List<Person>> GetFilteredPersons(Expression<Func<Person, bool>> predicate);

        /// <summary>  
        /// Updates a person object in the data store  
        /// </summary>  
        /// <param name="person">Person object to update</param>  
        /// <returns>Returns the updated person object</returns>  
        Task<Person> UpdatePerson(Person person);


        /// <summary>
        /// Deletes a person object from the data store based on the given person id
        /// </summary>
        /// <param name="personId">personId guid to search</param>
        /// <returns>Return true if the deletion is successfull otherwise false</returns>
        Task<bool> DeletePerson(Guid personId);
    }
}
