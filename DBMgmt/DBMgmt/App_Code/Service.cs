using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data;


public class Service : IService
{
	/*Enacpsulating query functionality to call SQL server 
			Recieves: string: Representing SQL syntax for query 
			Returns	: string: Query resuly for DB
	*/
	public string Query(string query)
	{
		
		string result = ""; //The value to be returned after SQL call
		string connStr =
			"Data Source=localhost\\SQLEXPRESS01;" +
			"Initial Catalog=mstart_hiring_task;" +
			"User ID=REST_SVC;Password=REST#n1";
		try
		{
			// Trying to make connection to DB in case of 
			// failuare return that as a response.
			using (SqlConnection connection =
					   new SqlConnection(connStr))
			{
				SqlCommand command =
					new SqlCommand(query, connection);

				connection.Open();

				// Executing command
				SqlDataReader reader = command.ExecuteReader();

				// Reading rows, row by row and adding them to result string
				while (reader.Read())
				{
					int fieldsCount = reader.FieldCount;

					for (int i = 0; i < fieldsCount; i++)
					{
						result += reader.GetValue(i).ToString();
						if (i != fieldsCount - 1) result += ", ";
					}
					result += "\n";
				}

				reader.Close();
			}
		}

		//Redirecting error messages to the response
		catch (SqlException e) { return e.Message; }
		catch (Exception e) { return e.Message; }

		return result;
	}


	/*
	 * Enacpsulating query functionality to call SQL server 
		Recieves: string: Representing SQL syntax to modify the database values
		Returns	: string: Either return an error message OR Sucessful Execution & number of rows effected
	*/

	public string Modify(string row)
	{
		//Connection string for SQL DB
		string connStr =
			"Data Source=localhost\\SQLEXPRESS01;" +
			"Initial Catalog=mstart_hiring_task;" +
			"User ID=REST_SVC;Password=REST#n1";
		try
		{
			using (SqlConnection connection =
					   new SqlConnection(connStr))
			{
				SqlCommand command =
					new SqlCommand(row, connection);

				connection.Open(); // Connecting to DB
				int rowsEffected = command.ExecuteNonQuery(); // Executing SQL command

				if (rowsEffected == 0)
					return ""; // If nothing changed no rows are effected return empty string

				// Return a message confirming sucess and number of rows effected
				return "Sucessful Execution - No. of rows effected," + rowsEffected ;
			}
		}

		// return any error as SQL error
		catch (Exception e) { return e.Message;}
	}

}
