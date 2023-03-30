using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace MStart_Hiring_Task.Controllers
{
    public class DealOperations
    {
        // This class interfaces the endpoints of the DealMgmt REST API with each 

        //Method making REST call to DealMgmt API to add a deal returning boolean
        // marking the sucess or failuare of the adding operation
        public static bool AddDeal(string name,string descr,string status,int amount,string currency) 
        {

            // Calling database management REST API and returning either true or false 
            //  for adding the deal
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =

                   "http://localhost:5005/dealmgmt/service.svc/addDeal?" +
                    "name="     + name +
                   "&descr="    + descr +
                   "&status="   + status +
                   "&amount="   + amount +
                   "&currency=" + currency;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { return false; }
                catch (Exception) { return false; }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                response = response.Split(',')[1];

                int rowsEffected = Int32.Parse(response);

                if (rowsEffected > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
        }

        //Method making REST call to DealMgmt API to claim a deal returning boolean
        // marking the sucess or failuare of the claiming of a deal
        public static bool claimDeal(int userID, int dealID, int amount, string currency)
        {

            // Calling database management REST API and returning either true or false 
            //  for adding the deal
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                // Actual call to the API.
                string urlString =

                   "http://localhost:5005/dealmgmt/service.svc/claimDeal?" +
                    "UserID=" + userID +
                   "&DealID=" + dealID +
                   "&amount=" + amount +
                   "&currency=" + currency;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { return false; }
                catch (Exception) { return false; }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                response = response.Split(',')[1];

                int rowsEffected = Int32.Parse(response);

                if (rowsEffected > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }

        }

        //Method making REST call to DealMgmt API to change a deal status
        // the return values is a flag for the sucess or failuare of this operation
        public static bool ChangeStatus(int id,string status)
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =

                   "http://localhost:5005/dealmgmt/service.svc/setStatus?" +
                    "id=" + id +
                   "&status=" + status;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { return false; }
                catch (Exception) { return false; }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                response = response.Split(',')[1];

                int rowsEffected = Int32.Parse(response);

                if (rowsEffected > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }

        }

        //Method making REST call to DealMgmt API to get all the deals in the format of 
        // rows seperated by '\n' and columns(Values) seperated by ',' using a nested 
        // loop to iterate throuhg the deals.
        public static string getAllDeals()
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =
                    "http://localhost:5005/dealmgmt/Service.svc/getAllDeals";

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { }
                catch (Exception) { }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                return response;
            }
            catch (Exception) { return "Invalid call/ Empty table !"; }
        }

        //Method making REST call to DealMgmt API to get all the claimed deals in the format of 
        // rows seperated by '\n' and columns(Values) seperated by ',' using a nested 
        // loop to iterate throuhg the deals.
        public static string getAllClaimedDeals()
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =
                    "http://localhost:5005/dealmgmt/service.svc/getallclaimeddeals";

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { }
                catch (Exception) { }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                return response;
            }
            catch (Exception) { return "Invalid call/ Empty table !"; }
        }

        //Method making REST call to DealMgmt API to get all claimed deals for a user format of 
        // rows seperated by '\n' and columns(Values) seperated by ',' using a nested 
        // loop to iterate throuhg the deals.
        public static string getUserClaimedDeals(int ID)
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =
                    "http://localhost:5005/dealmgmt/service.svc/getUserClaimedDeals?UserID=" + ID;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { }
                catch (Exception) { }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                return response;
            }
            catch (Exception) { return "Invalid call/ Empty table !"; }
        }

        //Method making REST call to DealMgmt API to get a single deal using its ID
        // with columns seperated by ','
        public static string getDeal(int DealID)
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =
                    "http://localhost:5005/dealmgmt/service.svc/getDeal?ID=" + DealID;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { }
                catch (Exception) { }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                return response;
            }
            catch (Exception e) { return e.Message; }
        }

        //Method making REST call to UserMgmt API to get user ID using their email
        // Thanks to the sleeplessness this method should've been in the UsersOperations class
        // but now its here. Oh Wow! 
        public static int getUserID(string email)
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =
                    "http://localhost:5005/usermgmt/service.svc/getuserId?email=" + email;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { }
                catch (Exception) { }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                return Int32.Parse(response);
                
            }
            catch (Exception) { return -1; }
        }

    }
}
