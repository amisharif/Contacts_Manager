using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class CountryResponse
    {
        public Guid? CountryID { get; set; }
        public string? CountryName { get; set; }

        //It compares the current object to another object of CountryResponse type and returns true, if both values are same; otherwise returns false
        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }
            CountryResponse country_to_compare = (CountryResponse)obj;

            return CountryID == country_to_compare.CountryID && CountryName == country_to_compare.CountryName;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }


    public static class CountryExtension
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse()
            {
                CountryID = country.CountryID,
                CountryName = country.CountryName,
               
            };
        }
    }
}
