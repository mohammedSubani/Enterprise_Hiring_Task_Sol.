using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;
using System.Net;
using System.Web;
using System.Text.RegularExpressions;

public class Service : IService
{
	/**
	 * This file encapsulates any operations that need to be done to manage 
	 * deals table on SQL DB using DB Management API (dbMgmt) REST API.
	 * 
	 * All operations are basiclly a calls to two endpoints Query and Modify
	 * **/

	/*	Get a deal row by ID number 
	 *	Recieves: int ID number for the deal
	 *  Returns: Row of that deal with values seperated by ','
	 */
	public string getDeal(int ID)
	{
		// SQL Query syntax to get a deal row 
		string SqlStr = "select ID,Name,Description,Status,Amount,Currency " +
						"from deals where ID = " + ID;

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			// Calling the endpoint for DBMgmt with endpoint Query, parameter is the SQL Query
			string urlString =
			   "http://localhost:5005/DBMgmt/Service.svc/query?str=" + SqlStr;

			try { responseDoc.LoadXml(client.DownloadString(urlString)); }

			catch (WebException) { return "Cannot connect to DBMgmt service"; }
			catch (Exception e)  { return e.Message; }
		}

		string response = "";

		// Getting string value embedded in XML response
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


	/*	Set a deal status by ID number 
	 *	Recieves: int ID number for the deal and string for status
	 *  Returns:  string message either sucess or failuare of SQL command
	*/
	public string setStatus(int ID,string status) 
	{
		// Validating empty string calls
		if (status == "")
			return "Empty fields";

		// Removing any undesired spaces or trainling spaces
		status = status.Trim();
		status = status.Replace(" ", "");

		// SQL Query command for the deals table 
		string SqlStr = "UPDATE deals " +
						"SET Status = \'" + status + "\' " +
						"WHERE ID = " + ID;

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			// Calling the endpoint for DBMgmt with endpoint Query, parameter is the SQL Query

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
			return "No matched deal in database";
		else
		{
			response = response.Trim();
			return response;
		}
	}

	/*	Adding a deal with all of its assoicated data
	 *	Recieves: int ID number for the deal and string for status
	 *  Returns:  string message either sucess or failuare of SQL command
	*/
	public string addDeal(string name, string descr, string status,
						  int amount, string currency) 
	{
		//Validate empty fields
		if (name == "" || descr == "" || status == ""
			|| currency == "")
			return "Empty fields";

		//Validate name field for only letters and spaces(Regex Validation)
		if (!Regex.Match(name, "^[a-zA-Z][a-zA-Z ]*$").Success)
			return "Please enter a valid name (Letters only!)";


		// SQL Insert statement
		string SqlStr = "INSERT INTO deals (" +
			"Name," +
			"Description," +
			"Status," +
			"Amount," +
			"Currency" +
			")";

		SqlStr += "Values (" +
						  "\'" + name + "\'," +
						  "\'" + descr + "\'," +
						  "\'" + status + "\'," +
						         amount + "," +
						  "\'" + currency + "\'" +
						  ")";

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			// Actual call to the endpoint Modify of DBMgmt API to add row
			string urlString =
			   "http://localhost:5005/DBMgmt/Service.svc/modify?str=" + SqlStr;

			try { responseDoc.LoadXml(client.DownloadString(urlString)); }

			catch (WebException) { return "Cannot connect to DBMgmt service"; }
			catch (Exception e)  { return e.Message; }
		}

		XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
		string response = elemList[0].InnerXml;

		return response;
	}

	/*	Deleting a deal's by its ID 
	 *	Recieves: int ID number for the deal
	 *  Returns:  string message either sucess or failuare of SQL command
	*/
	public string deleteDeal(int ID) 
	{
		// SQL Query to delete the targeted row
		string SqlStr = "Delete From deals Where ID=" + ID;

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			// Calling endpoint Modify to DBMgmt API
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
			return "No matched deal in database";
		else
		{
			response = response.Trim();
			return response;
		}
	}

	/*	Getting all the deals in the table
	 *	Recieves: Void
	 *  Returns:  string message with all rows where rows are seperated by '\n' 
	 *			  and columns(values) seperated by ','
	*/
	public string getAllDeals() 
	{
		// SQL query for all rows with desired data to recieve
		string SqlStr = "select ID,Name,Description,Status,Amount,Currency " +
						"from deals" ;

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			//Actual call to DBMgmt API using Query endpoint
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


	/*	Getting  claimed deal in claimed deals table using their ID
	 *	Recieves: int of claimed deal ID
	 *  Returns:  string message with columns(values) seperated by ','
	 *			  
	*/
	public string getClaimedDeal(int ID) 
	{
		// SQL Query
		string SqlStr = "select ID,User_ID,Deal_ID,Amount,Currency " +
						"from claimedDeals where ID = " + ID;

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			string urlString =
			   "http://localhost:5005/DBMgmt/Service.svc/query?str=" + SqlStr;

			try { responseDoc.LoadXml(client.DownloadString(urlString)); }

			catch (WebException) { return "Cannot connect to DBMgmt service"; }
			catch (Exception e)  { return e.Message; }
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

	/*	Getting all claimed deals in claimed deals table
		*  Recieves: int ID of the claimed deal
		*  Returns:  string message with all rows where rows are seperated by '\n' 
		*			  and columns(values) seperated by ','
	*/
	public string getAllClaimedDeal()
	{
		// SQL query with desired data values for claimed deals
		string SqlStr = "select ID,User_ID,Deal_ID,Amount,Currency " +
						"from claimedDeals" ;

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			// Acutal call to DBMgmt API using Query endpoint
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
			return response;
		}
	}

	/*		Getting all claimed deals for a user using user's ID
    	*	Recieves: int ID of the user
	    *   Returns:  string message with all rows where rows are seperated by '\n' 
    	*			  and columns(values) seperated by ','
	*/
	public string getUserClaimedDeals(int UserID) 
	{
		// SQL Query for a claimed deal for a user
		string SqlStr = "select ID,User_ID,Deal_ID,Amount,Currency " +
						"from claimedDeals where User_ID = " + UserID;

		//Calling database management service
		XmlDocument responseDoc = new XmlDocument();

		using (WebClient client = new WebClient())
		{
			// Calling DB Mgmt API using Query endpoint
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

	/*		Making a deal claimed by adding it to the claimed deals table
		*	Recieves: int ID for user , int ID for the deal ID , int amount for the deal and string currency
		*   Returns:  string message with message of either sucess or failuare for adding the deal
	*/
	public string claimDeal(int UserID, int DealID, int Amount, string Currency) 
	{
		//Validate empty fields
		if (UserID <= 0 || DealID <= 0 || Amount <= 0
			|| Currency == "" || Currency == null)
			return "Invalid input";


		// SQL Insert statement
		string SqlStr = "INSERT INTO claimedDeals (" +
			"User_ID," +
			"Deal_ID," +
			"Amount," +
			"Currency" +
			")";

		SqlStr += "Values (" +
						   UserID + "," +
						   DealID + "," +
						   Amount + "," +
						  "\'" + Currency + "\'" +
						  ")";

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

		XmlNodeList elemList = responseDoc.GetElementsByTagName("string");
		string response = elemList[0].InnerXml;

		return response;
	}
}
