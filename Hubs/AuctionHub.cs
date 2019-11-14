using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using practice.Models;
using System.Threading;
using Microsoft.AspNet.SignalR.Hubs;

namespace practice.Hubs
{
    [HubName("Auction")]
    public class AuctionHub : Hub
    {
        #region Properties
        static private Timer timer;
        public static bool initialized = false;
        public static object initLock = new object();
        //AuctionViewModel is used to store and keep track of bid information
        static public AuctionViewModel auctionViewModel;
        public static int secs_10 = 10000;
        public static int sec = 1000;
        #endregion

        #region Initialization
        public AuctionHub()
        {
            if (initialized)
                return;

            lock (initLock)
            {
                if (initialized)
                    return;

                InitializeAuction();
            }
        }

        private void InitializeAuction()
        {
            // Initialize model
            auctionViewModel = new AuctionViewModel(0, 10, DateTime.Now.AddSeconds(59), 10);

            timer = new System.Threading.Timer(TimerExpired, null, sec, 0);

            initialized = true;
        }
        #endregion

        #region ServerToClient
        /// <summary>
        /// Thread method to show the remaining time till the end of the bid
        /// It send bid results when the bid is over and update clients to prevent new bids
        /// </summary>
        /// <param name="state"></param>
        public void TimerExpired(object state)
        {
            if (auctionViewModel.TimeRemaining > 0)
            {
                Clients.All.updateRemainingTime(string.Format("{0:hh\\:mm\\:ss}", auctionViewModel.GetTimeRemaining()));
                timer.Change(sec, 0);
            }
            else
            {
                timer.Dispose();
                Clients.All.updateRemainingTime("00:00:00");
                Clients.All.finishBidding();
                AddMessage("Time Expired");
                if (!String.IsNullOrEmpty(auctionViewModel.LastUserBid))
                    AddMessage(string.Format("Congratulations {0}! \n {0} has won the auction with {1}$ \n on {2}", auctionViewModel.LastUserBid, auctionViewModel.ValueLastBid, auctionViewModel.LastBid));
                else
                    AddMessage(string.Format("time expired! product remains unsold !"));
            }
        }

        /// <summary>
        /// Server messages to clients. Now it only report bid results
        /// </summary>
        /// <param name="msg">The message</param>
        public void AddMessage(string msg)
        {
            Clients.All.addMessage(msg);
        }

        /// <summary>
        /// Called after each client's bid
        /// </summary>
        /// <param name="user">user name of last bid</param>
        /// <param name="value">the value of last bid</param>
        public void NotifyNewBid(string user, decimal value)
        {
            Clients.All.notifyNewBid(string.Format("{0} did a new bid of {1:c} at {2:T}", user, value, DateTime.Now));
        }
        #endregion

        #region ClientToServer
        /// <summary>
        /// Called by client when pressing bid button
        /// </summary>
        /// <param name="valueBid">The value of the bid (last bid + 1 cent)</param>
        /// <param name="user">The user name</param>
        public void PlaceBid(string valueBid, string user)
        {
            auctionViewModel.PlaceBid(decimal.Parse(valueBid), user);

            CallRefresh();

            NotifyNewBid(user, decimal.Parse(valueBid));
        }

        /// <summary>
        /// This is the method called by clients to retrieve information about bid current status
        /// </summary>
        public void CallRefresh()
        {
            Clients.All.auctionRefresh(auctionViewModel);
        }
        #endregion

    }
}