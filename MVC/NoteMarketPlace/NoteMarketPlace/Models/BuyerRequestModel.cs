using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class BuyerRequestModel
    {
        public Download Downloadstbl { get; set; }
        public User Userstbl { get; set; }
        public UserProfile UserProfiletbl { get; set; }


    }
}