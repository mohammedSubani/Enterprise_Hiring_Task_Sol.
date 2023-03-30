using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MStart_Hiring_Task.Models
{
    public class Deal
    {
        // A class representing the deals data structure
        public int ID { get; set; }

        public string name { get; set;}

        public string description { get; set;}

        public string status { get; set;}

        public int amount { get; set;}
        public string currency { get; set;}
    }
}
