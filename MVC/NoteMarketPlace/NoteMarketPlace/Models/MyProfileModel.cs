using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class MyProfileModel
    {
        public int ID { get; set; }



        [Required(ErrorMessage = "FirstName is required")]
        [MaxLength(50, ErrorMessage = "FirstName length should be <50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string FirstName { get; set; }



        [Required(ErrorMessage = "LastName is required")]
        [MaxLength(50, ErrorMessage = "LastName length should be <50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string LastName { get; set; }


        public string Email { get; set; }


        [Required(ErrorMessage = "Phone No. is required")]
        [Phone]
        [MaxLength(10, ErrorMessage = "Phone No. length should be <10")]
        public string PhoneNumber { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DOB { get; set; }

        public HttpPostedFileBase ProfilePicture { get; set; }

        public string PpPath { get; set; }

        [Required(ErrorMessage = "Address1 is required")]
        [MaxLength(100, ErrorMessage = "Address1 length should be <100")]
        public string AddressOne { get; set; }

        [Required(ErrorMessage = "Address2 is required")]
        [MaxLength(100, ErrorMessage = "Address2 length should be <100")]
        public string AddressTwo { get; set; }

        [Required(ErrorMessage = "City name is required")]
        [MaxLength(50, ErrorMessage = "City name length should be <50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string City { get; set; }

        [Required(ErrorMessage = "State name is required")]
        [MaxLength(50, ErrorMessage = "State name length should be <50")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string State { get; set; }

        [Required(ErrorMessage = "Zipcode is required")]
        [MaxLength(50, ErrorMessage = "Zipcode length should be <50")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Enter natural number please")]
        public string ZipCode { get; set; }

        [MaxLength(100, ErrorMessage = "Universityname should be <100")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string University { get; set; }

        [MaxLength(100, ErrorMessage = "Collegename should be <100")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string College { get; set; }

        
        [Required(ErrorMessage = "Country is required")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Countrycode is required")]
        public string CountryPhoneCode { get; set; }

        public Nullable<int> Gender { get; set; }

        public IEnumerable<ReferenceData> GenderList { get; set; }

        public IEnumerable<Country> CountryList { get; set; }


    }
}