using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace practice.Models
{
    public class AuctionViewModel
    {
        [Key]
        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public double ProductPrice { get; set; }

        
        [DisplayName("Product Image")]
        public string ImagePath { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public string ProductInfo { get; set; }



          public int TimeRemaining
          {
            get
            {
                return (int)Math.Abs(GetTimeRemaining().TotalSeconds);
            }
        }
        public double BidPrice { get; set; }

        [NotMapped]
        public string TypeTime { get; set; }
        [NotMapped]
        public int IncrementTimePerBid { get; set; }
        [NotMapped]
        public int BidsTotal { get; set; }
        [NotMapped]
        public DateTime LastBid { get; set; }
        [NotMapped]
        public DateTime EndTime { get; set; }
        [NotMapped]
        public decimal ValueLastBid { get; set; }
        [NotMapped]
        public decimal ValueNextBid { get { return ValueLastBid + (decimal)0.01; } }
        [NotMapped]
        public string LastUserBid { get; set; }

        public string EndTimeFullText
         {
            get { return string.Format("{0:MM/dd/yyyy HH\\:mm\\:ss}", EndTime); }
        }

        public AuctionViewModel() { }

        public AuctionViewModel(int bidsTotal, int incrementTimePerBid, DateTime endTime, decimal valueInitialBid)
        {
            BidsTotal = bidsTotal;
            IncrementTimePerBid = incrementTimePerBid;
            LastBid = DateTime.Now;
            EndTime = endTime;
            ValueLastBid = valueInitialBid;
        }

        public TimeSpan GetTimeElapsed()
        {
            return DateTime.Now.Subtract(LastBid);
        }

         public TimeSpan GetTimeRemaining()
        {
            return DateTime.Now.Subtract(EndTime);
        }
    
        public void SetEndTime()
        {
            EndTime += TimeSpan.FromSeconds(IncrementTimePerBid);
        }


        public void PlaceBid(decimal valueLastBid, string lastUserBid)
        {
            ValueLastBid = valueLastBid;
            LastUserBid = lastUserBid;
            BidsTotal++;
            LastBid = DateTime.Now;
            SetEndTime();
        }
    
    }
}