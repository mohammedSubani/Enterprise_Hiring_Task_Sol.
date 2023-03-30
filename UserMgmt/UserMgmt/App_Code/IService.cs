using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Net.Mail;

[ServiceContract]
public interface IService
{
	[OperationContract]
	[WebGet(UriTemplate = "Register?name={name}&email={email}&phoneNumber={phoneNumber}&" +
						  "password={password}&gender={gender}&birth={birth}")]
	string Register
		(string name,
		 string email,
		 string phoneNumber,
		 string password,
		 string gender,
		 string birth);

	[OperationContract]
	[WebGet(UriTemplate = "Authenticate?email={email}&password={password}")]
	string Authenticate(string email, string password);

	[OperationContract]
	[WebGet(UriTemplate = "Authorize?email={email}")]
	string Authorize(string email);

	[OperationContract]
	[WebGet(UriTemplate = "ChangeStatus?email={email}&status={status}")]
	string ChangeStatus(string email,string status);

	[OperationContract]
	[WebGet(UriTemplate = "DeleteUser?email={email}")]
	string DeleteUser(string email);

	[OperationContract]
	[WebGet(UriTemplate = "getAllUsers")]
	string GetAllUsers();

	[OperationContract]
	[WebGet(UriTemplate = "getUserID?email={email}")]
	string GetUserID(string email);

	[OperationContract]
	[WebGet(UriTemplate = "SetProfilePic?url={url}&email={email}")]
	string SetProfilePic(string url,string email);
}


