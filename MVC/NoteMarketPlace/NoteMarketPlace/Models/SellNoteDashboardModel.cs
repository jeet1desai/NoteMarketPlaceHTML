using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class SellNoteDashboardModel
    {
        public IEnumerable<SellerNote> InProgressNote { get; set; }
        public IEnumerable<SellerNote> PublishedNote { get; set; }

        public int MyDownload { get; set; }
        public int NumberOfSoldNote { get; set; }
        public int MoneyEarned { get; set; }
        public int MyRejectedNote { get; set; }
        public int BuyerRequest { get; set; }


    }
}