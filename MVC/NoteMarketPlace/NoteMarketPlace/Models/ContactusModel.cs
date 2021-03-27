using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ContactusModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string FullName { get; set; }



        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }




        [Required]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string Subject { get; set; }



        [Required]
        public string Comments { get; set; }
    }
}