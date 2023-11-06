using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnT.Models
{


    public class ProgressHub : Hub
    {
        public string message = "Initializing and Preparing...";
        public int OverallProgress = 0;
        public int batchProgress = 0;

        public static void sendMessage(string message, int OverallProgress, int batchProgress)
        {

            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            hubContext.Clients.All.sendMessageToPage(message, OverallProgress, batchProgress);
        }

        public void GetCountAndMessage2()
        {
            Clients.Caller.sendMessageToPage(string.Format(message), OverallProgress, batchProgress);
        }


    }
}