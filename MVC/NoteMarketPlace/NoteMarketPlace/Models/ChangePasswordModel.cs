using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class ChangePasswordModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,24}$", ErrorMessage = "Enter Valid Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "New Password is required")]
        public string NewPassword { get; set; }

        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,24}$", ErrorMessage = "Enter Valid Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Passowrd is required")]
        public string ConfirmPassword { get; set; }


    }
}