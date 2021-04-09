using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class AdminProfileModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(50, ErrorMessage = "FirstName length should be <50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }



        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(50, ErrorMessage = "LastName length should be <50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }



        public string Email { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string SecondEmail { get; set; }

        [Required]
        public string PhoneCode { get; set; }


        [Required(ErrorMessage = "Phone No. is required")]
        [Phone]
        [MaxLength(10, ErrorMessage = "Phone No. length should be <10")]
        public string Phone { get; set; }

        
        public HttpPostedFileBase ProfilePicture { get; set; }

        public IEnumerable<Country> CountryList { get; set; }
    }
}