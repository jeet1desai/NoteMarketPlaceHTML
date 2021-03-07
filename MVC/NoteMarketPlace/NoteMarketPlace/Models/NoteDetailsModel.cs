using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class NoteDetailsModel
    {

        public SellerNote sellerNote { get; set; }
        public SellerNotesAttachement sellerNotesAttachement { get; set; }

        public bool DisableBtn { get; set; }
    }
}