using Enities;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Runtime.CompilerServices;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Represents DTO class that is used as return type of most methods of Person Service
    /// </summary>
    public class PersonResponse
    {
        public Guid PersonId { get; set; }

        public string? PersonName { get; set; }

        public string? Email { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public Guid? CountryID { get; set; }

        public string? Country { get; set; }

        public string? Address { get; set; }

        public string? ReceiveNewsLetters { get; set; }

        public string? Age { get; set; }

        /// <summary>
        /// Compares the current object data with the object passed as parameter
        /// </summary>
        /// <param name="obj"> The PersonResponse Object to compare with</param>
        /// <returns>True or False, Indicating whether all person details are matched with the specified parameter object </returns>
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(PersonResponse)) return false;

            PersonResponse person = (PersonResponse)obj;

            return PersonId == person.PersonId &&
                    PersonName == person.PersonName &&
                    Email == person.Email &&
                    DateOfBirth == person.DateOfBirth &&
                    Gender == person.Gender &&
                    CountryID == person.CountryID &&
                    Country == person.Country &&
                    ReceiveNewsLetters == person.ReceiveNewsLetters &&
                    Age == person.Age &&
                    Address == person.Address;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //override the ToString method to return the person details
        public override string ToString()
        {
            return $"PersonId: {PersonId}, " +
                   $"PersonName: {PersonName}, " +
                   $"Email: {Email}, " +
                   $"DateOfBirth: {DateOfBirth?.ToString("dd MMM yyyy")}, " +
                   $"Gender: {Gender}, " +
                   $"CountryID: {CountryID}, " +
                   $"Country: {Country}, " +
                   $"Address: {Address}, " +
                   $"ReceiveNewsLetters: {ReceiveNewsLetters}, " +
                   $"Age: {Age}";
        }
        public PersonUpdateRequest ToPerosnUpdateRequest()
        {
            return new PersonUpdateRequest()
            {
                PersonID = PersonId,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = string.IsNullOrEmpty(Gender) ? null : Enum.Parse(typeof(GenderOptions), Gender, true) as GenderOptions?,
                CountryID = CountryID,
                Address = Address,
                ReceiveNewsLetters = !string.IsNullOrEmpty(ReceiveNewsLetters) && bool.Parse(ReceiveNewsLetters)
            };
        }

    }
}

public static class PersonExtensions
{
    /// <summary>
    /// An Extension method to covert an object of person class into personResponse class
    /// </summary>
    /// <param name="person">The Person object to convert</param>
    /// <returns>Returns the converted PersonResponse object</returns>
    public static PersonResponse ToPersonResponse(this Person person)
    {
        return new PersonResponse()
        {
            PersonId = person.PersonID,
            PersonName = person.PersonName,
            Email = person.Email,
            DateOfBirth = person.DateOfBirth,
            Gender = person.Gender,
            CountryID = person.CountryID,
            Country = person.Country?.CountryName ?? "Unknown",
            Address = person.Address,
            ReceiveNewsLetters = person.ReceiveNewsLetters.ToString(), ////?
            Age = person.DateOfBirth.HasValue
    ? Math.Round((DateTime.Now - person.DateOfBirth.Value).TotalDays / 365.25).ToString()
    : null

        };
    }
}
