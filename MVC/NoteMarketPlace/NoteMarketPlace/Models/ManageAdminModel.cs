using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ManageAdminModel
    {
        public int ID { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [Required]
        public string FirstName { get; set; }


        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        [Required]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required]
        public string Email { get; set; }


        [Required]
        [Phone]
        [MaxLength(10, ErrorMessage = "Phone No. length should be <10")]
        public string Phone { get; set; }


        [Required]
        public string CountryPhoneCode { get; set; }

        public IEnumerable<Country> CountryList { get; set; }
    }
}