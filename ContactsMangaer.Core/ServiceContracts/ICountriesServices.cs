using ServiceContracts.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating countries
    /// </summary>
    public interface ICountriesServices
    {
        /// <summary>
        /// Adds a new country to the database.
        /// </summary>
        /// <param name="countryAddRequest">The country object to add</param>
        /// <returns>Returns the country object after adding it, including the newly generated country ID</returns>
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

        /// <summary>
        /// Retrieves all countries from the database.
        /// </summary>
        /// <returns>A list of all countries as <see cref="CountryResponse"/> objects</returns>
        Task<List<CountryResponse>> GetAllCountries();

        /// <summary>
        /// Retrieves a country object based on the given country ID.
        /// </summary>
        /// <param name="countryId">The unique identifier of the country</param>
        /// <returns>The matching country as a <see cref="CountryResponse"/> object, or null if not found</returns>
        Task<CountryResponse?> GetCountryByCountryId(Guid countryId);
    }
}
