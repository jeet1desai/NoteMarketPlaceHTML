using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class EditNoteModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(100, ErrorMessage = "<100 char")]
        public string Title { get; set; }




        [Required(ErrorMessage = "Category is required")]
        public int Category { get; set; }



        public HttpPostedFileBase DisplayPicture { get; set; }



        public HttpPostedFileBase[] UploadNotes { get; set; }



        public Nullable<int> NoteType { get; set; }



        public Nullable<int> NumberofPages { get; set; }



        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }



        [MaxLength(200, ErrorMessage = "<200 char")]
        public string UniversityName { get; set; }



        public Nullable<int> Country { get; set; }



        [MaxLength(100, ErrorMessage = "<100 char")]
        public string Course { get; set; }



        [MaxLength(100, ErrorMessage = "<100 char")]
        public string CourseCode { get; set; }



        [MaxLength(100, ErrorMessage = "<100 char")]
        public string Professor { get; set; }



        [Required(ErrorMessage = "Select the Selling Mode")]
        public bool IsPaid { get; set; }



        public Nullable<decimal> SellingPrice { get; set; }


        public HttpPostedFileBase NotesPreview { get; set; }


        public IEnumerable<NoteCategory> NoteCategoryList { get; set; }


        public IEnumerable<NoteType> NoteTypeList { get; set; }


        public IEnumerable<Country> CountryList { get; set; }



        public string DpPathName { get; set; }
        public string NpPathName { get; set; }
        public string NtPathName { get; set; }
    }
}