using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService" in both code and config file together.
[ServiceContract]
public interface IService
{

	[OperationContract]
	[WebGet(UriTemplate = "getDeal?ID={ID}")]
	string getDeal(int ID);

	[OperationContract]
	[WebGet(UriTemplate = "setStatus?ID={ID}&status={status}")]
	string setStatus(int ID,string status);

	[OperationContract]
	[WebGet(UriTemplate = "addDeal?name={name}&descr={descr}&status={status}&" +
								  "amount={amount}&currency={currency}")]
	string addDeal(string name,string descr,string status,
				   int amount,string currency);

	[OperationContract]
	[WebGet(UriTemplate = "deleteDeal?ID={ID}")]
	string deleteDeal(int ID);

	[OperationContract]
	[WebGet(UriTemplate = "getAllDeals")]
	string getAllDeals();

	[OperationContract]
	[WebGet(UriTemplate = "getClaimedDeals?ID={ID}")]
	string getClaimedDeal(int ID);

	[OperationContract]
	[WebGet(UriTemplate = "getAllClaimedDeals")]
	string getAllClaimedDeal();

	[OperationContract]
	[WebGet(UriTemplate = "getUserClaimedDeals?UserID={UserID}")]
	string getUserClaimedDeals(int UserID);

	[OperationContract]
	[WebGet(UriTemplate = "claimDeal?UserID={UserID}&DealID={DealID}&Amount={Amount}&Currency={Currency}")]
	string claimDeal(int UserID,int DealID,int Amount,string Currency);
}


