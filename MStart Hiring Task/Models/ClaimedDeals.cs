using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MStart_Hiring_Task.Models
{
    public class ClaimedDeals
    {
        // A class representing the claimed deals data structure
        public int ID { get; set; }
        public int UserId { get; set; }
        public int DealID { get; set; }
        public int Amount { get; set; }
        public string Currency { get; set; }
    }
}
