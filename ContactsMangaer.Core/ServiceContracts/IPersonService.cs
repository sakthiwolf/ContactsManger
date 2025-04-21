using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ServiceContracts
{
    /// <summary>
    /// Represents bussiness logic for  manipulating persons entities
    /// </summary>
    public interface IPersonService
    {
        /// <summary>
        /// Add a new person object to the list of persons
        /// </summary>
        /// <param name="personAddRequest">Person object to add</param>
        /// <returns>Retuens the person objectr after adding it (including newly Generated person id</returns>
        Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// retuen the  list of all persons
        /// </summary>
        /// <returns>All persons form the list as list of PersonResponse</returns>
        Task  <List<PersonResponse>> GetAllPersons();

        /// <summary>
        /// Retrun the person object based on the given perosn id
        /// </summary>
        /// <param name="personId">Person id to Search</param>
        /// <returns>Return the match Person object</returns>
        Task<PersonResponse?> GetPersonByPersonId (Guid? personId);

        /// <summary>
        /// Returns all person objects that matches with the given seach field and 
        /// search string
        /// </summary>
        /// <param name="searchBy">Search field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on
        /// the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        /// <summary>
        /// Returns sorted list of persons based on the given sort field and sort order
        /// </summary>
        /// <param name="allPersons">Represents list ofpersons to sort</param>
        /// <param name="sortyBy">Name of the property(Key),based on whci the persons should be sorted</param>
        /// <param name="sortOrder">ASC Or DESC</param>
        /// <returns>Return sorted person as personResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortyBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Update the specified person details based on the given personId
        /// </summary>
        /// <param name="personUpdateRequest">Person details to update,including personId</param>
        /// <returns>Return the person response object after updation</returns>
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);


        /// <summary>
        /// Delete the person object based on the given person id
        /// </summary>
        /// <param name="personId">PersonId to delete</param>
        /// <returns>Retuens true, if the deletion is successfull otherwise false</returns>
        Task<bool> DeletePerson(Guid? personId);


        /// <summary>
        /// Retrun person as CSV
        /// </summary>
        /// <returns>Return the memory strem with CSV data</returns>
        Task<MemoryStream> GetPersonCSV();

        /// <summary>
        /// Returns persons as EXcel
        /// </summary>
        /// <returns>Returns the memory stream with Excel data of persons</returns>
        Task<MemoryStream> GetPersonExcel();
    }
}
