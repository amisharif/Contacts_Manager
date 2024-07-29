using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CountriesServiceTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService(false);
        }


        #region Add Country
        [Fact]
        public void AddCountry_Null_Country()
        {

            CountryAddRequest country = null;

            Assert.Throws<ArgumentNullException>(() =>
            {
                _countriesService.AddCountry(country);
            });
        }


        [Fact]
        public void AddCountry_Null_CountryName()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = null,
            };
            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(countryAddRequest);
            });
        }

        [Fact]
        public void AddCountry_Duplicate_CountryName()
        {
            CountryAddRequest country1 = new CountryAddRequest()
            {
                CountryName = "USA",
            };
            CountryAddRequest country2 = new CountryAddRequest()
            {
                CountryName = "USA",
            };


            Assert.Throws<ArgumentException>(() =>
            {
                _countriesService.AddCountry(country1);
                _countriesService.AddCountry(country2);
            });
        }

        //When you supply proper country name, it should insert (add) the country to the existing list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest() { CountryName = "Japan" };

            //Act
            CountryResponse response = _countriesService.AddCountry(request);
            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //Assert
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);

        }

        #endregion

        #region Get All Country

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();
            Assert.Empty(actual_country_response_list);

        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
           {
               new(){ CountryName="USA"},
               new() {CountryName = "CANADA"},
               new() {CountryName = "FINLAND"}
           };

            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

            foreach (CountryAddRequest item in country_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(item));
            }

            List<CountryResponse> actual_country_response_list = _countriesService.GetAllCountries();

            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actual_country_response_list);

            }
        }

        #endregion


        [Fact]

        public void GetCountryByCountryID_NullCountryID()
        {
            Guid? countryID = null;

            CountryResponse country_response_from_get_method =  _countriesService.GetCountryByCountryID(countryID);
            Assert.Null(country_response_from_get_method);
        }


        [Fact]
        public void GetCountryByCountryID_ValidCountryID()
        {
            CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "USA" };

            CountryResponse country_response_from_add_request = _countriesService.AddCountry(countryAddRequest);

            CountryResponse? country_response_from_get = 
                _countriesService.GetCountryByCountryID(country_response_from_add_request.CountryID);

            Assert.Equal(country_response_from_add_request, country_response_from_get);
        }
    }
}