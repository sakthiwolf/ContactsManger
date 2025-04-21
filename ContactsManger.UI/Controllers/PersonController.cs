using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ContactMangerApp.Controllers
{
    [Route("[controller]")]
    public class PersonController : Controller
    {
      
        //private field
        private readonly IPersonService _personService;
        private readonly ICountriesServices _countriesServices;

        public PersonController(IPersonService personService, ICountriesServices countriesServices)
        {
            _personService = personService;
            _countriesServices = countriesServices;

        }


        [Route("[action]")] //url:person/create
        [Route("/")]
        public async Task <IActionResult> Index(string searchBy, string? searchString,
                             string sortBy = nameof(PersonResponse.PersonName),
                             SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            // Search Fields
        ViewBag.SearchFields = new Dictionary<string, string>(){
        { nameof(PersonResponse.PersonName), "Person Name" },
        { nameof(PersonResponse.Email), "Email" },
        { nameof(PersonResponse.DateOfBirth), "Date Of Birth" },
        { nameof(PersonResponse.CountryID), "Country Id" },
        { nameof(PersonResponse.Address), "Address" },
        { nameof(PersonResponse.Gender), "Gender" }
    };

            // Get filtered persons
            List<PersonResponse> persons = await _personService.GetFilteredPersons(searchBy, searchString);
            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            // Sorting
            List<PersonResponse> sortedPersons =  await _personService.GetSortedPersons(persons, sortBy, sortOrder);
            ViewBag.CurrentSortedPersons = sortedPersons;
            ViewBag.CurrentSortBy = sortBy; // Added this to track sorting field
            ViewBag.CurrentSortOrder = sortOrder.ToString(); // Store as string for comparison

            return View(sortedPersons);
        }

        //Execute when the user clicks on "Create person" hyperlink
        //(while opening the create view)
      
        [HttpGet]
        [Route("[action]")] //url:person/create
        public async Task <IActionResult> Create()
        {
         List<CountryResponse> countries=  await _countriesServices.GetAllCountries();
         ViewBag.Countries = countries;
            //new SelectListItem {Text="harsha",Valu="1" }

            return View();
        }

        //This Create method used for when user submit the form,to request to CreateAction[Post]
        [HttpPost]
        [Route("[action]")] //url:person/create
        [HttpPost]
        public async Task <IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Countries = _countriesServices.GetAllCountries();
                ViewBag.Errors = ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList();
                return View();
            }

            // Call the service method to add person
            PersonResponse personResponse = await _personService.AddPerson(personAddRequest);

            // Redirect to the Index page after successful creation
            return RedirectToAction("Index", "Person");
        }


        //To Get the data from indexview clickby PersonId //and opening a edit view this method will wrks[GET}
        [HttpGet]
        [Route("[action]/{personID}")]//eg:/person/edit/1
        public async Task<IActionResult> Edit(Guid personID)
        {
            //get persondata get by person id
         PersonResponse? personResponse=  await _personService.GetPersonByPersonId(personID);
            if(personResponse == null)
            {
                return RedirectToAction("Index");
            }
            //update the persondata
            PersonUpdateRequest personUpdateRequest = personResponse.ToPerosnUpdateRequest();

            List<CountryResponse> countries = await _countriesServices.GetAllCountries();
            ViewBag.Countries = countries;
            return View(personUpdateRequest);
        }


        //When the user click Update btn   post request // when user click update btn this method will wrls[POST]
        [HttpPost]
        [Route("[action]/{personID}")]//eg:/person/edit/1
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            //To ged personId form hiddeninputfield
           PersonResponse? personResponse= await   _personService.GetPersonByPersonId(personUpdateRequest.PersonID);

            if (personResponse == null) {
                return RedirectToAction("Index");
            
            }

            if (ModelState.IsValid) { 

               PersonResponse?    updatedPerson= await  _personService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");

            }
            else
            {
                ViewBag.Countries = _countriesServices.GetAllCountries();
                ViewBag.Errors = ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage)
                                            .ToList();
                return View(personResponse.ToPerosnUpdateRequest()); 

            }



           
        }


        //When the user click Delete btn in index view[GET]
        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task <IActionResult> Delete(Guid? personId)
        {
            PersonResponse? presonResponse=  await _personService.GetPersonByPersonId(personId);
            if (presonResponse == null) 
            {
              return RedirectToAction("Index");
            }
            return View(presonResponse);
        
        }


        //When the user click Deletebtn in delete view [POST] 
        [HttpPost]
        [Route("[action]/{personID}")]
        public  async Task <IActionResult>Delete(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personService.GetPersonByPersonId(personUpdateRequest.PersonID);

            if (personResponse == null) {

                return RedirectToAction("Index");
            }
           await  _personService.DeletePerson(personUpdateRequest.PersonID);
            return RedirectToAction("Index");

          

        }



        //for Pdf
        [Route("PersonPDF")]//person/personPDF
        public async Task <IActionResult> PersonPDF()
        {
         //get list of person
         List<PersonResponse> persons=   await _personService.GetAllPersons();
            //return view as pdf

            return new ViewAsPdf("PersonPDF", persons, ViewData)
            {
                PageMargins= new Rotativa.AspNetCore.Options.Margins()
                {
                    Top=20, Right=20, Bottom=20, Left=20
                },
                PageOrientation= Rotativa.AspNetCore.Options.Orientation.Landscape
            };

        }

        [Route("PersonCSV")]//person/personCSV
        public async Task<IActionResult> PersonCSV()
        {
            MemoryStream memoryStream =   await   _personService.GetPersonCSV();

            return File(memoryStream, "application/octet-stream", "persons.csv");
        }


        [Route("PersonExcel")]//person/personCSV
        public async Task<IActionResult> PersonExcel()
        {
            MemoryStream memoryStream = await _personService.GetPersonExcel();

        
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PersonReport.xlsx");

        }



    }
}
