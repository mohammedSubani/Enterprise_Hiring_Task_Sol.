using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MStart_Hiring_Task.Models
{
    public class User
    {
        // A class representing the users data structure
        public int ID       {get; set;}
        public string name  {get; set;}

        public string email { get; set; }

        public string phone { get; set; }

        public string status { get; set; }

        public string gender { get; set; }

        public string dateOfBirth { get; set; }
    }
}
