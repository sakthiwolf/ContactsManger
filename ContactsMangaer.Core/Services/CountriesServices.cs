
using ServiceContracts;
using ServiceContracts.DTO;

using Enities;
using RepositroyContracts;

namespace Services
{
    public class CountriesServices : ICountriesServices
    {
        // Private field
        //private readonly PersonsDbContext _db;
        private readonly IcountryRepository _countriesRepositroy;

        public CountriesServices(IcountryRepository countriesRepositroy)
        {
            _countriesRepositroy = countriesRepositroy;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            // Validation: CountryAddRequest should not be null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            // Validation: CountryName should not be null or empty
            if (string.IsNullOrWhiteSpace(countryAddRequest.CountryName))
            {
                throw new ArgumentNullException(nameof(countryAddRequest.CountryName), "Country name cannot be null or empty.");
            }

            // Validation: CountryName should not be duplicate
            if (await _countriesRepositroy.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Country name should not be duplicate.");
            }

            // Convert object from CountryAddRequest to Country type
            Country country = countryAddRequest.ToCountry();

            // Generate a new ID for the country
            country.CountryId = Guid.NewGuid();

            // Add the country to the database
            await _countriesRepositroy.AddCountry(country);
            //await _countriesRepositroy.SaveChangesAsync();

            // Return the country object after adding it
            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            List<Country> countries = await _countriesRepositroy.GetAllCountries();
            return  countries
                .Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid countryId)
        {
            // Validation: CountryId should not be empty
            if (countryId == Guid.Empty)
            {
                throw new ArgumentException("Country ID cannot be empty.", nameof(countryId));
            }

            // Get the matching country based on the given countryId
            var country = await _countriesRepositroy.GetCountryByCountryId(countryId);

            // Convert matching country object to CountryResponse type
            return country?.ToCountryResponse();
        }

    }
}
