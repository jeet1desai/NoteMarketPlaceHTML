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

        public int Spam { get; set; }  

        public int AvgRating { get; set; }

        public int TotalReview { get; set; }

        public IEnumerable<ReviewsModel> notesreview { get; set; }

    }

    public class ReviewsModel
    {
        public User Users { get; set; }
        public UserProfile UserProfiles { get; set; }
        public SellerNotesReview SellerNotesReviews { get; set; }
    }
}