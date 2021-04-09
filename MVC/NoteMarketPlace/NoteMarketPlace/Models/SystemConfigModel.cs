using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class SystemConfigModel
    {

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Phone is required")]
        [MaxLength(10, ErrorMessage = "Phone No. length should be <10")]
        [Phone]
        public string Phone { get; set; }


        [Required(ErrorMessage = "SecondaryEmail is required")]
        public string SecondaryEmail { get; set; }


        [Required(ErrorMessage = "FacebookUrl is required")]
        public string FacebookUrl { get; set; }


        [Required(ErrorMessage = "TwitterUrl is required")]
        public string TwitterUrl { get; set; }


        [Required(ErrorMessage = "LinkedinUrl is required")]
        public string LinkedinUrl { get; set; }


        public HttpPostedFileBase ImageNote { get; set; }


        public HttpPostedFileBase ImageProfile { get; set; }
    }
}