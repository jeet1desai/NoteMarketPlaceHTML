using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace NoteMarketPlace.Models
{
    public class AddNoteModel
    {

        public int ID { get; set; }




        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage ="<100 char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string Title { get; set; }




        [Required(ErrorMessage = "Category is required")]
        public int Category { get; set; }




        public HttpPostedFileBase DisplayPicture { get; set; }




        [Required(ErrorMessage = "Note is required")]
        public HttpPostedFileBase[] UploadNotes { get; set; }



        public Nullable<int> NoteType { get; set; }



        public Nullable<int> NumberofPages { get; set; }




        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }




        [MaxLength(200, ErrorMessage = "<200 char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string UniversityName { get; set; }




        public Nullable<int> Country { get; set; }




        [MaxLength(100, ErrorMessage = "<100 char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string Course { get; set; }





        [MaxLength(100, ErrorMessage = "<100 char")]
        public string CourseCode { get; set; }





        [MaxLength(100, ErrorMessage = "<100 char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Use letters only please")]
        public string Professor { get; set; }




        [Required(ErrorMessage = "Select the Selling Mode")]
        public bool IsPaid { get; set; }





        public Nullable<decimal> SellingPrice { get; set; }





        [Required(ErrorMessage = "NotePreview is required")]
        public HttpPostedFileBase NotesPreview { get; set; }




        public IEnumerable<NoteCategory> NoteCategoryList { get; set; }

        public IEnumerable<NoteType> NoteTypeList { get; set; }

        public IEnumerable<Country> CountryList { get; set; }
        
    }
}
