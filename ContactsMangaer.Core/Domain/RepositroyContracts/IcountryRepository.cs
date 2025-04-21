using Enities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositroyContracts
{
    public interface IcountryRepository
    {
        /// <summary>
        /// adds a new country object to the data store.
        /// </summary>
        /// <param name="country">country object to add</param>
        /// <returns>returns the county object after adding it to the data store</returns>
        Task<Country>AddCountry(Country country);

        /// <summary>
        /// Returns all countries in the data store 
        /// </summary>
        /// <returns>All countries form the table</returns>
        Task<List<Country>> GetAllCountries();

        /// <summary>
        /// Return a country object based on the given country id 
        /// otherwise returns null
        /// </summary>
        /// <param name="countryId">CountryId to search</param>
        /// <returns>matching country or null</returns>
        Task<Country?> GetCountryByCountryId(Guid countryId);


        /// <summary>
        /// Returns a country object based on the given country name
        /// </summary>
        /// <param name="countryName"></param>
        /// <returns>matching country or null</returns>
        Task<Country?> GetCountryByCountryName(string countryName);

    }
}
