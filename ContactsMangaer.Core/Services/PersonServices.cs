
using CsvHelper;
using Enities;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using RepositroyContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;
using System.Drawing;
using System.Globalization;


namespace Services
{
    public class PersonServices : IPersonService
    {
        //private readonly PersonsDbContext _Db;
        //private readonly ICountriesServices _countriesServices;


        private readonly IPersonRepositroy _personRepositroy;

        // Add the missing private field for _countriesServices  
        private readonly ICountriesServices _countriesServices;



        public PersonServices(IPersonRepositroy personRepository)
        {
            _personRepositroy = personRepository;
           
        }



        //Convet the person object into personResponse type
        //To Get the Country Name from the CountryId form CountryService
        private async Task<PersonResponse> ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            var countryResponse = person.CountryID.HasValue ? await _countriesServices.GetCountryByCountryId(person.CountryID.Value) : null;
            personResponse.Country = countryResponse?.CountryName;
            return personResponse;
        }



        //Add a new person to the list of persons
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }
            //Model Validation
            ValidationHelper.ModelValidation(personAddRequest);

            //convert object form PersonAddRequest to Person type /to give model object
            Person person = personAddRequest.ToPerson();

            //generate a new id for the person
            person.PersonID = Guid.NewGuid();

            //add the person to the list of persons
            await _personRepositroy.AddPerson(person);
         //await    _personRepositroy.SaveChangesAsync();

            //Convert the person object into personResponse Type
            return  await ConvertPersonToPersonResponse(person);

        }

        //Get all the persons from the list of persons
        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _personRepositroy.GetAllPersons();
            var personResponses = new List<PersonResponse>();

            foreach (var person in persons)
            {
                personResponses.Add(await ConvertPersonToPersonResponse(person));
            }

            return personResponses;
        }

        //Get the person object based on the given person id
        public async Task <PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            //Check if the person id is null
            if (!personId.HasValue)
            {
                return null;
            }
            //Get the person object based on the person id
            Person? person = await _personRepositroy.GetPersonByPersonID(personId.Value);
            if (person == null)
            {
                return null;
            }

            //Convert the person object into personResponse Type
            //return person.ToPersonResponse();
            return await ConvertPersonToPersonResponse(person);

            //Return the matching country as "CountryResponse" object
        }

        //Get all the persons that matches with the given search field and search string
        public  async Task <List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> AllPersons = await GetAllPersons();
            List<PersonResponse> MatchingPersons = AllPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return MatchingPersons;

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    MatchingPersons = AllPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ?
                    temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    MatchingPersons = AllPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ?
                    temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    MatchingPersons = AllPersons.Where(temp => (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMM yyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    MatchingPersons = AllPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;


                case nameof(PersonResponse.CountryID):
                    MatchingPersons = AllPersons.Where(temp => (temp.CountryID.HasValue ?
                    temp.CountryID.Value.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    MatchingPersons = AllPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default:
                    MatchingPersons = AllPersons;
                    break;

            }
            return MatchingPersons;


        }


        //Get the sorted list of persons based on the given sort field and sort order
        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortyBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortyBy))
                return allPersons;

            List<PersonResponse> SortedPersons = await Task.Run(() =>
            {
                return (sortyBy, sortOrder) switch
                {
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),

                    (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
                    (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),

                    (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.ReceiveNewsLetters).ToList(),
                    (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.ReceiveNewsLetters).ToList(),

                    _ => allPersons
                };
            });

            return SortedPersons;
        }

        //Update Persondata
        public  async Task <PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest));
            }

            if (personUpdateRequest.PersonID == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personUpdateRequest.PersonID), "Person ID can't be blank");
            }

            // Model Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            // get matching person object to update based on the personId
            Person? MatachingPerson = await _personRepositroy.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (MatachingPerson == null)
            {
                throw new ArgumentException("Given person id does not exist");
            }

            // Check if the CountryID already exists in the database for another person
            //if (_Db.Persons.Any(p => p.CountryID == personUpdateRequest.CountryID && p.PersonID != personUpdateRequest.PersonID))
            //{
            //    throw new ArgumentException("The specified CountryID already exists.");
            //}

            // Update the person object based on the given personUpdateRequest
            MatachingPerson.PersonName = personUpdateRequest.PersonName;
            MatachingPerson.Email = personUpdateRequest.Email;
            MatachingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            MatachingPerson.Address = personUpdateRequest.Address;
            MatachingPerson.Gender = personUpdateRequest.Gender?.ToString();
            MatachingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            MatachingPerson.CountryID = personUpdateRequest.CountryID;

            await _personRepositroy.UpdatePerson(MatachingPerson);//update

            return await ConvertPersonToPersonResponse(MatachingPerson);
        }


        //DeletePerosn
        // DeletePerson
        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? matchingPerson = await _personRepositroy.GetPersonByPersonID(personId.Value);

            if (matchingPerson == null)
            {
                return false;
            }

            await _personRepositroy.DeletePerson(personId.Value);
            return true;
        }



        //To Generate CSC file
        public async Task<MemoryStream> GetPersonCSV()
        {
           MemoryStream memoryStream = new MemoryStream();
           StreamWriter streamWriter= new StreamWriter(memoryStream);
           CsvWriter csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen: true);

            csvWriter.WriteHeader<PersonResponse>();// personId,personName...

            csvWriter.NextRecord();
            List<PersonResponse> persons = await GetAllPersons();
                  //.Persons
                //.Include("Country")
                //.Select(temp => temp.ToPersonResponse()).ToList();

            await csvWriter.WriteRecordsAsync(persons);

            memoryStream.Position = 0;

            return memoryStream;


        }

        // Fix for the CS0029 error in the GetPersonExcel method.  
        public async Task<MemoryStream> GetPersonExcel() //EpPlus Package for more documentation  
        {
            // Set EPPlus License Context (required for non-commercial use)  
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            // Create MemoryStream  
            MemoryStream memoryStream = new MemoryStream();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                // Create a worksheet  
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");

                // Define Header Row  
                string[] headers = { "Person Name", "Email", "Date of Birth", "Age", "Gender", "Country", "Address", "Receive Newsletters" };

                // Apply Styles to Header Row  
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true; // Bold text  
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.LightGray); // Background color  
                    worksheet.Cells[1, i + 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Center align  
                    worksheet.Cells[1, i + 1].Style.Border.BorderAround(ExcelBorderStyle.Thin); // Border around cells  
                }

                // Fetch persons data  
                List<PersonResponse> personResponses = await GetAllPersons();

                // Populate worksheet with data  
                int row = 2; // Start from the second row (below headers)  
                foreach (PersonResponse person in personResponses)
                {
                    worksheet.Cells[row, 1].Value = person.PersonName;
                    worksheet.Cells[row, 2].Value = person.Email;
                    worksheet.Cells[row, 3].Value = person.DateOfBirth?.ToString("yyyy-MM-dd"); // Nullable check  
                    worksheet.Cells[row, 4].Value = person.Age;
                    worksheet.Cells[row, 5].Value = person.Gender;
                    worksheet.Cells[row, 6].Value = person.Country ?? "N/A"; // Handle null country  
                    worksheet.Cells[row, 7].Value = person.Address;
                    worksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

                    // Apply Border to Each Row  
                    for (int col = 1; col <= headers.Length; col++)
                    {
                        worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                        worksheet.Cells[row, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                    }

                    row++;
                }

                // Autofit columns  
                worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

                // Save Excel package  
                await excelPackage.SaveAsync();
            }

            // Reset stream position  
            memoryStream.Position = 0;
            return memoryStream;
        }

        // Update the constructor to initialize _countriesServices  
        public PersonServices(IPersonRepositroy personRepository, ICountriesServices countriesServices)
        {
            _personRepositroy = personRepository;
            _countriesServices = countriesServices;
        }




        //public async Task<MemoryStream> GetPersonExcel()
        //{
        //    // Set EPPlus License Context (required for non-commercial use)
        //    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        //    // Create MemoryStream
        //    MemoryStream memoryStream = new MemoryStream();

        //    using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
        //    {
        //        // Create a worksheet
        //        ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");

        //        // Header row
        //        worksheet.Cells["A1"].Value = "Person Name";
        //        worksheet.Cells["B1"].Value = "Email";
        //        worksheet.Cells["C1"].Value = "Date of Birth";
        //        worksheet.Cells["D1"].Value = "Age";
        //        worksheet.Cells["E1"].Value = "Gender";
        //        worksheet.Cells["F1"].Value = "Country";
        //        worksheet.Cells["G1"].Value = "Address";
        //        worksheet.Cells["H1"].Value = "Receive Newsletters";

        //        // Fetch persons data with country included
        //        List<Person> persons = await _Db.Persons
        //        .Include(p => p.Country)  // Use strongly-typed Include
        //         .ToListAsync();


        //        // Convert to response objects
        //        List<PersonResponse> personResponses = persons.Select(temp => temp.ToPersonResponse()).ToList();

        //        // Populate worksheet
        //        int row = 2;  // Start from second row (below headers)
        //        foreach (PersonResponse person in personResponses)
        //           {
        //            worksheet.Cells[row, 1].Value = person.PersonName;
        //            worksheet.Cells[row, 2].Value = person.Email;
        //            worksheet.Cells[row, 3].Value = person.DateOfBirth?.ToString("yyyy-MM-dd");  // Nullable check
        //            worksheet.Cells[row, 4].Value = person.Age;
        //            worksheet.Cells[row, 5].Value = person.Gender;
        //            worksheet.Cells[row, 6].Value = person.Country ?? "N/A";  // Handle null country
        //            worksheet.Cells[row, 7].Value = person.Address;
        //            worksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;

        //            row++;
        //        }

        //        // Autofit columns
        //        worksheet.Cells[$"A1:H{row}"].AutoFitColumns();

        //        // Save Excel package
        //        await excelPackage.SaveAsync();
        //    }

        //    // Reset stream position
        //    memoryStream.Position = 0;
        //    return memoryStream;
        //}


    }
}

