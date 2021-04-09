using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class AdminMemberModel
    {
        public User User { get; set; }
        public int NoteUnderReview { get; set; }
        public int PublishedNote { get; set; }
        public int DownloadedNote { get; set; }
        public int TotalExpanses { get; set; }
        public int TotalEarning { get; set; }

    }
}