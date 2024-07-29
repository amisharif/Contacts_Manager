using Entities;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required]
        public string? PersonName { get; set; }
        [Required]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson()
        {
            return new Person() { PersonName = PersonName, Email = Email, 
                DateOfBirth = DateOfBirth, Gender = Gender.ToString(), 
                Address = Address, CountryID = CountryID, ReceiveNewsLetters = ReceiveNewsLetters };
        }
    }
}
