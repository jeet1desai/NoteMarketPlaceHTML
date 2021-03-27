using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class LoginModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        
        [Required]
        public string Password { get; set; }

        public bool IsSelected { get; set; }
    }
}