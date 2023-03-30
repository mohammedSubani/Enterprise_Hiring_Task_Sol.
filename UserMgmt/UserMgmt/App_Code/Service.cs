using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Net;
using System.Xml;
using System.Data.SqlClient;
using System.IO;

public class Service : IService
{
    /**
      * This file encapsulates any operations that need to be done to manage 
      * users table on SQL DB using DB Management API (dbMgmt) REST API.
      * 
      * All operations are basiclly a calls to two endpoints Query and Modify
      * **/

    /*	Register a new user by adding the user data to users table
	 *	Recieves: string name,string email,string,phone, string password,string gender 
	 *	          ,string date of birth
	 *  Returns: Message of either sucess or failed registration
	 */
    public string
        Register(string name, string email, string phoneNumber,
                 string password, string gender, string birth)
    {
        //Validate empty fields
        if (name == "" || email == "" || phoneNumber == ""
            || gender == "" || password == "" || birth == "")
            return "Empty fields";

        //Validate name field for only letters and spaces(Regex Validation)
        if (!Regex.Match(name, "^[a-zA-Z][a-zA-Z ]*$").Success)
            return "Please enter a valid name (Letters only!)";

        // Validate email using built-in function 
        if (!IsValidEmail(email))
            return "Incorrect email format";

        //Validate password THIS IS NOT TESTED !
        if (password.Contains(' '))
            return "No white spaces allowed!";


        //Validate birth date using DateTime class
        DateTime birthDate;

        try { birthDate = DateTime.Parse(birth); }
        catch (Exception) { return "Invalid date format"; }

        // SQL Insert statement
        string SqlStr = "INSERT INTO Users (" +
            "Name," +
            "Email," +
            "Phone," +
            "Status," +
            "Password," +
            "Gender," +
            "Date_of_birth," +
            "role"+
            ")";

        SqlStr += "Values (" +
                          "\'" + name + "\'," +
                          "\'" + email + "\'," +
                          "\'" + phoneNumber + "\'," +
                          "'Active'," +
                          "\'" + password + "\'," +
                          "\'" + gender + "\'," +
                          "\'" + birthDate.ToString() + "\'," +
                          "'User'" +
                          ")";

        //Calling database management service
        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/modify?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException)   { return "Cannot connect to DBMgmt service"; }
            catch (Exception e)    { return e.Message; }
        }

        XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
        string response = elemList[0].InnerXml;

        return response;
    }

    /*	Using user's email and password to authenticate their access to certain pages
     *	Recieves: string email and string passowrd
     *  Returns: Message of either sucess or failed authentication
     */
    public string Authenticate(string email, string password)
    {
        if (email == "" || password == "")
            return "Empty fields!";

        // Validate email using built-in function 
        if (!IsValidEmail(email))
            return "Incorrect email format";

        // Making sure password has no empty spacse
        if (password.Contains(' '))
            return "No white spaces allowed";

        // SQL Query to check for the user getting back the assoicated password and checking for it
        string SqlStr = "select email,password from users where email= " + "\'" +email+ "\'";

        //Calling database management service
        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/query?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException) { return "Cannot connect to DBMgmt service"; }
            catch (Exception e)  { return e.Message;}
        }

        try
        {
            string response = "";

            XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
            response = elemList[0].InnerXml;

            if (response == "" || response == null)
                return "No matched email in database";
            else 
            {


                response = response.Replace(" ", "");
                response = response.Trim();


                if (response.Split(',')[1] == password)
                    return "Authenticated";
                else
                    return "not Authenticated";

            }
        }

        catch (Exception) { return "Some unexpected error happened, code needs debugging !!!"; }
    }

    /*	Using user's email to get their role (Admin OR user) authorization
    *	Recieves: string email
    *   Returns:  Message of either sucess or failed authorization
    */
    public string Authorize(string email)
    {
        // Validate for empty fields
        if (email == "")
            return "Empty fields";

        // Validate email using built-in function 
        if (!IsValidEmail(email))
            return "Incorrect email format";

        // SQL Query
        string SqlStr = "select role from users where email = " + "\'" +email+ "\'";

        //Calling database management service
        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/query?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException) { return "Cannot connect to DBMgmt service"; }
            catch (Exception e)  { return e.Message;}
        }

        string response = "";

        XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
        response = elemList[0].InnerXml;

        if (response == "" || response == null)
            return "No matched email in database";
        else 
        {
            response = response.Trim();
            return response;
        }  
    }


    /*	Changing user status (Active,Inactive ....)
     *	Recieves: string email
     *  Returns:  Message of either sucess or failed changing of status
    */
    public string ChangeStatus(string email, string status)
    {
        // Validate for empty fields
        if (email == "" || status == "")
            return "Empty fields";

        // Validate email using built-in function 
        if (!IsValidEmail(email))
            return "Incorrect email format";

        // SQL Query
        string SqlStr = "UPDATE Users " +
                        "SET Status = \'" + status + "\' " +
                        "WHERE email like " + "\'" + email + "\'";

        //Calling database management service
        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/modify?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException) { return "Cannot connect to DBMgmt service"; }
            catch (Exception e)  { return e.Message; }
        }

        string response = "";

        XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
        response = elemList[0].InnerXml;

        if (response == "" || response == null)
            return "No matched email in database";
        else
        {
            response = response.Trim();
            return response;
        }
    }

    /*	 Deleting a user using their unique email address
      *	 Recieves: string email
      *  Returns:  Message of either sucess or failed delete operation
    */
    public string DeleteUser(string email)
    {
        if (email == "")
            return "Empty fields";

        // Validate email using built-in function 
        if (!IsValidEmail(email))
            return "Incorrect email format";



        // SQL Query
        string SqlStr = "DELETE FROM Users WHERE email=" + "\'" + email + "\'";

        //Calling database management service
        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/modify?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException) { return "Cannot connect to DBMgmt service"; }
            catch (Exception e) { return e.Message; }
        }

        string response = "";

        XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
        response = elemList[0].InnerXml;

        if (response == "" || response == null)
            return "No matched email in database";
        else
        {
            response = response.Trim();
            return response;
        }
    }

    /*	 Get all the users from users table with their associated data 
      *	 rows are seperated by '\n' and columns are seperated by ','
      *	 Recieves: void
      *  Returns:  String of all users table rows
    */
    public string GetAllUsers() 
    {
        // SQL Query
        string SqlStr = "select ID,Name,Email,Phone,Status,Gender,Date_of_birth " +
                        "from users";

        //Calling database management service
        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/query?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException) { return "Cannot connect to DBMgmt service"; }
            catch (Exception e) { return e.Message; }
        }

        string response = "";

        XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
        response = elemList[0].InnerXml;

        if (response == "" || response == null)
            return "No matched deal in database";
        else
        {
            response = response.Trim();
            return response;
        }
    }

    /*	 Get user data row by their ID
     *	 rows are seperated by '\n' and columns are seperated by ','
     *	 Recieves: void
     *   Returns:  String of all users table rows
    */

    public string GetUserID(string email)
    {
        // SQL Query
        string SqlStr = "select ID " +
                        "from users where email = " + "\'" + email + "\'";

        //Calling database management service
        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/query?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException) { return "Cannot connect to DBMgmt service"; }
            catch (Exception e) { return e.Message; }
        }

        string response = "";

        XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
        response = elemList[0].InnerXml;

        if (response == "" || response == null)
            return "No matched deal in database";
        else
        {
            response = response.Trim();
            return response;
        }
    }

      /*	 Adding an image to profilesPic table for users
        *	 Recieves: string image path for the profile pic, string email of user
        *    Returns:  Message with sucess or failuare result of the call
      */
    public string SetProfilePic(string imagePath, string email)
    {
        string SqlStr = "INSERT INTO ProfilePics (" +
            "email," +
            "imageUrl" +
            ")";

        SqlStr += "Values (" +
                          "\'" + email + "\'," +
                          "\'" + imagePath + "\'" +
                          ")";

        XmlDocument responseDoc = new XmlDocument();

        using (WebClient client = new WebClient())
        {
            string urlString =
               "http://localhost:5005/DBMgmt/Service.svc/modify?str=" + SqlStr;

            try { responseDoc.LoadXml(client.DownloadString(urlString)); }

            catch (WebException) { return "Cannot connect to DBMgmt service"; }
            catch (Exception e) { return e.Message; }
        }

        string response = "";

        XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
        response = elemList[0].InnerXml;

        if (response == "" || response == null)
            return "No matched ID in database";
        else
        {
            response = response.Trim();
            return response;
        }
    }

    // Using built-in method to validate email format
    public static bool IsValidEmail(string emailaddress)
    {
        try
        {
            MailAddress m = new MailAddress(emailaddress);

            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }

}
