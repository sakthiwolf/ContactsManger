using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public class PersonRequestHelper
    {
        // <summary>
        /// Creates a new PersonAddRequest with the provided details.
        /// </summary>
        public static PersonAddRequest CreatePersonRequest(
            string name, string email, string address, GenderOptions gender,
            string dob, bool receiveNewsletters, Guid countryId)
        {
            return new PersonAddRequest()
            {
                PersonName = name,
                Email = email,
                Address = address,
                Gender = gender,
                DateOfBirth = DateTime.Parse(dob),
                ReceiveNewsLetters = receiveNewsletters,
                CountryID = countryId // Linking with country
            };
        }
    }
}
