using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace ContactsManager.Controllers
{

    [Route("[controller]")]
    public class PersonsController : Controller
    {

        private readonly IPersonsService _personsService;
        private readonly ICountriesService _countriesService;

        public PersonsController(IPersonsService personsService, ICountriesService countriesService)
        {
            _personsService = personsService;
            _countriesService = countriesService;
        }

        [Route("/")]
        [Route("[action]")]
        public IActionResult Index(string searchBy, string? searchString,
            string sortBy = nameof(PersonResponse.PersonName),
            SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>()
            {
            { nameof(PersonResponse.PersonName), "Person Name" },
            { nameof(PersonResponse.Email), "Email" },
            { nameof(PersonResponse.DateOfBirth), "Date of Birth" },
            { nameof(PersonResponse.Gender), "Gender" },
            { nameof(PersonResponse.CountryID), "Country" },
            { nameof(PersonResponse.Address), "Address" }
            };



            ViewBag.CurrentSearchBy = searchBy;
            ViewBag.CurrentSearchString = searchString;

            List<PersonResponse> persons = _personsService.GetFilteredPersons(searchBy, searchString);

            List<PersonResponse> sortedPerson = _personsService.GetSortedPersons(persons, sortBy, sortOrder);

            ViewBag.CurrentSortBy = sortBy;
            ViewBag.CurrentSortOrder = sortOrder.ToString();


            return View(sortedPerson);
        }

        [HttpGet]
        [Route("/Persons/Create")]
        public IActionResult Create()
        {
            List<CountryResponse> countries = _countriesService.GetAllCountries();

            ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() }
            );
            return View();
        }


        [HttpPost]
        [Route("[action]")]
        public IActionResult Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries = _countriesService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                return View();
            }

            PersonResponse personResponse = _personsService.AddPerson(personAddRequest);

            return RedirectToAction("Index", "Persons");
        }



        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Edit(Guid personID)
        {

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);

            

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }
            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            return View(personUpdateRequest);
        }


        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Edit(PersonUpdateRequest personUpdateRequest)
        {

            List<CountryResponse> countries = _countriesService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            PersonResponse? personResponse = _personsService.UpdatePerson(personUpdateRequest);
            if (personResponse == null) return View();

            return RedirectToAction("Index");
        }


        [HttpGet]
        [Route("[action]/{personID}")]
        public IActionResult Delete(Guid personID)
        {

            PersonResponse? personResponse = _personsService.GetPersonByPersonID(personID);
            if (personResponse == null) { return  RedirectToAction("Index"); }
            return View(personResponse);
        } 
        

        [HttpPost]
        [Route("[action]/{personID}")]
        public IActionResult Delete(PersonUpdateRequest personUpdateRequest)
        {

            if(personUpdateRequest == null) { RedirectToAction("Index"); return View(); }
            _personsService.DeletePerson(personUpdateRequest.PersonID);

            return RedirectToAction("Index");
        }


    }
}
