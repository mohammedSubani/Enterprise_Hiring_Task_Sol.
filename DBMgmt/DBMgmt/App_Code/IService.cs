using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

[ServiceContract]
public interface IService
{
	[OperationContract]
	[WebGet(UriTemplate ="query?str={str}")]
	string Query(string str);


	[OperationContract]
	[WebGet(UriTemplate = "modify?str={str}")]
	string Modify(string str);
}

