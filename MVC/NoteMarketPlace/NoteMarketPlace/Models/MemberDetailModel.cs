using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class MemberDetailModel
    {
        public User User { get; set; }
        public UserProfile UserProfile { get; set; }

        public string CountryName { get; set; }

        public IEnumerable<MemberNotes> Notes { get; set; }
    }

    public class MemberNotes
    {
        public SellerNote SellerNote { get; set; }

        public int DownloadedNotes { get; set; }

        public int TotalEarning { get; set; }
    }
}