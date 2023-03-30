using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Web;
using System.Net;

namespace MStart_Hiring_Task.Controllers
{
    public class UsersOperations
    {
        // This class interfaces the endpoints of the UsersMgmt REST API with each 

        //Method making REST call to UserMgmt API to authenitcate user returning boolean
        // marking the sucess or failuare of authenticating a user
        public static bool AuthenticateUser(string email,string password) 
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =

                   "http://localhost:5005/usermgmt/Service.svc/Authenticate?" +
                    "email="    + email +
                   "&password=" + password;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { return false; }
                catch (Exception) { return false; }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                if (response == "Authenticated")
                    return true;
                else
                    return false;
            }
            catch (Exception) { return false; }
        }

        //Method making REST call to UserMgmt API to get users role returning string
        // of the role for that user (Admin or User)
        public static string GetAuthorization(string email)
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =

                   "http://localhost:5005/usermgmt/Service.svc/Authorize?" +
                    "email=" + email;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) {  }
                catch (Exception)    {  }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                return response;
            }
            catch (Exception) { return "Invalid authorization!"; }
        }

        //Method making REST call to UserMgmt API to register a new user 
        // with all the data required to fill the row in a talbe returning 
        // boolean marking the sucess or failuare of registering that user
        public static bool Register(string userName, string password,    string gender,
                                    string email,    string phoneNumber, string birthDate)
        {

            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =

                   "http://localhost:5005/usermgmt/Service.svc/register?" +
                    "name="       + userName +
                   "&email="      + email +
                   "&phoneNumber="+ phoneNumber +
                   "&password="   + password+
                   "&gender="     + gender+
                   "&birth="      + birthDate;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { return false; }
                catch (Exception)    { return false; }
            }

            try
            {
                XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
                string response = elemList[0].InnerXml;

                response = response.Split(',')[1];

                int rowsEffected =  Int32.Parse(response);

                if (rowsEffected > 0)
                    return true;
                else
                    return false;            
            }
            catch (Exception) {return false;}

        }

        //Method making REST call to UserMgmt API to change registered user returning 
        // boolean marking the sucess or failuare of altering the user status
        public static bool changeStatus(string email, string status)
        {

            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =

                   "http://localhost:5005/usermgmt/Service.svc/ChangeStatus?" +
                    "email="  + email +
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

        //Method making REST call to UserMgmt API to delete a registered user returning 
        // boolean marking the sucess or failuare of deleting the user from table
        public static bool DeleteUser(string email)
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =

                   "http://localhost:5005/usermgmt/Service.svc/deleteUser?" +
                    "email=" + email;

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

        //Method making REST call to UserMgmt API to get all the user in DB
        // returns all the users rows seperated by '\n' and columns(Values) 
        // seperated by ','
        public static string getAllUsers()
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =
                    "http://localhost:5005/usermgmt/service.svc/getallusers";

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


        //Method making REST call to UserMgmt API to set a  user profile picture
        // path with the assoicated user email in a table ProfilePics returning
        // boolean marking the sucess or failuare of deleting the user from table
        public static bool SetProfilePics(string imagePath,string email)
        {
            //Calling database management service
            XmlDocument responseDoc = new XmlDocument();

            using (WebClient client = new WebClient())
            {
                string urlString =
                    "http://localhost:5005/usermgmt/service.svc/setProfilePic?" +
                   "url=" + imagePath +
                   "&email=" + email;

                try { responseDoc.LoadXml(client.DownloadString(urlString)); }

                catch (WebException) { return false; }
                catch (Exception)    { return false; }
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

    }
}
