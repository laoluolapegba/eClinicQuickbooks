using developer.intuit.com;
using FCQB.Data;
using FCQBWebConnAPI.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;

namespace FCQBWebConnAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "QBWebConnAPI" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select QBWebConnAPI.svc or QBWebConnAPI.svc.cs at the Solution Explorer and start debugging.
    public class QBWebConnAPISoap : IQBWebConnAPISoap
    {
        #region Global Vars
        private static readonly ILog log = LogManager.GetLogger(typeof(QBWebConnAPISoap));
        eClatModel db;
        #endregion

        #region Constructor
        public QBWebConnAPISoap()
        {
            db = new eClatModel();
        }

        #endregion

        #region ServiceContracts
        /// IN: 
		/// string strUserName 
		/// string strPassword
		///
		/// OUT: 
		/// string[] authReturn
		/// Possible values: 
		/// string[0] = ticket
		/// string[1]
		/// - empty string = use current company file
		/// - "none" = no further request/no further action required
		/// - "nvu" = not valid user
		/// - any other string value = use this company file
		/// </summary>
        public authenticateResponse authenticate(authenticateRequest request)
        {
            ArrayOfString authReturn = new ArrayOfString();
            // Code below uses a random GUID to use as session ticket
            // An example of a GUID is {85B41BEE-5CD9-427a-A61B-83964F1EB426}
            authReturn.Add(System.Guid.NewGuid().ToString());
            string authUserName = ConfigurationManager.AppSettings["authUserName"].ToString();
            string authPassword = ConfigurationManager.AppSettings["authPassword"].ToString();
            
            //check is there is any work to do
            int count = db.debtor_trans.Where(a => a.sync_flag == false).Count();


            if (request.Body.strUserName.Trim().Equals(authUserName) && request.Body.strPassword.Trim().Equals(authPassword))
            {
                // An empty string for authReturn[1] means asking QBWebConnector 
                // to connect to the company file that is currently openned in QB
                authReturn[1] = ConfigurationManager.AppSettings["CompanyFileLocation"].ToString();
                //"c:\\Program Files\\Intuit\\QuickBooks\\sample_product-based business.qbw";
            }
            else
            {
                string operationvalue = string.Empty;
                operationvalue = count > 0 ? "nvu" : "none";
                authReturn.Add(operationvalue);
            }
            // You could also return "none" to indicate there is no work to do
            // or a company filename in the format C:\full\path\to\company.qbw
            // based on your program logic and requirements.
            authenticateResponse rsp = new authenticateResponse(new authenticateResponseBody(authReturn));
            return rsp;
        }

        public clientVersionResponse clientVersion(clientVersionRequest request)
        {
            string retVal = null;
            double recommendedVersion = double.Parse( ConfigurationManager.AppSettings["recommendedVersion"].ToString());
            double supportedMinVersion = double.Parse( ConfigurationManager.AppSettings["supportedMinVersion"].ToString());
            double suppliedVersion = Convert.ToDouble(this.parseForVersion(request.Body.strVersion));
            if (suppliedVersion < recommendedVersion)
            {
                retVal = "W:We recommend that you upgrade your QBWebConnector";
            }
            else if (suppliedVersion < supportedMinVersion)
            {
                retVal = "E:You need to upgrade your QBWebConnector";
            }
            clientVersionResponse rsp = new clientVersionResponse(new clientVersionResponseBody(retVal));
            return rsp;
        }

        public closeConnectionResponse closeConnection(closeConnectionRequest request)
        {
            throw new NotImplementedException();
        }

        public connectionErrorResponse connectionError(connectionErrorRequest request)
        {
            throw new NotImplementedException();
        }

        public getLastErrorResponse getLastError(getLastErrorRequest request)
        {
            int errorCode = 0;
            string retVal = null;
            if (errorCode == -101)
            {
                retVal = "QuickBooks was not running!"; // This is just an example of custom user errors
            }
            else
            {
                retVal = "Error!";
            }
            getLastErrorResponse rsp = new getLastErrorResponse(new getLastErrorResponseBody(retVal));
            return rsp;
        }

        public receiveResponseXMLResponse receiveResponseXML(receiveResponseXMLRequest request)
        {
            throw new NotImplementedException();
        }

        public sendRequestXMLResponse sendRequestXML(sendRequestXMLRequest request)
        {
            throw new NotImplementedException();
        }

        public serverVersionResponse serverVersion(serverVersionRequest request)
        {
            string serverVersion = "2.0.0.1";
            serverVersionResponse rsp = new serverVersionResponse();
            rsp.Body = new serverVersionResponseBody(serverVersion);
            return rsp;
        }

        private string parseForVersion(string input)
        {
            // This method is created just to parse the first two version components
            // out of the standard four component version number:
            // <Major>.<Minor>.<Release>.<Build>
            // 
            // As long as you get the version in right format, you could use
            // any algorithm here. 
            string retVal = "";
            string major = "";
            string minor = "";
            Regex version = new Regex(@"^(?<major>\d+)\.(?<minor>\d+)(\.\w+){0,2}$", RegexOptions.Compiled);
            Match versionMatch = version.Match(input);
            if (versionMatch.Success)
            {
                major = versionMatch.Result("${major}");
                minor = versionMatch.Result("${minor}");
                retVal = major + "." + minor;
            }
            else
            {
                retVal = input;
            }
            return retVal;
        }
        #endregion

    }
}
