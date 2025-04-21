using System;
using Enities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that is  used as return type for country of contriesService methods
    /// </summary>
    public class CountryResponse
    {
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        //It compare the current object to another object of countryresponse type and return true, 
        //if both are equal otherwise return false
        public override bool Equals(object? obj)
        {
            if(obj == null) return false;   
            if(obj.GetType()!=typeof(CountryResponse)) return false;
            CountryResponse country_to_compare = (CountryResponse)obj;
            return this.CountryId == country_to_compare.CountryId   &&
                   this.CountryName == country_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public static class ContryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryId = country.CountryId,
                CountryName = country.CountryName
            };
        }
    }
}
