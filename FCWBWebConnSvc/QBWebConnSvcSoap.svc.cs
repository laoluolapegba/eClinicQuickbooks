using developer.intuit.com;
using FCQB.Data;
using FCQBWebConnAPI.Model;
using FCQBWebConnAPI.Model.Bill;
using FCQBWebConnAPI.Model.Payment;
using FCQBWebConnAPI.Services;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using static FCQBWebConnAPI.Model.CustomerAddRsModel;

namespace FCQBWebConnAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.

    //Generate web   service from WSDL : wsdl first service
    //F:\LaoluOlapegba\Myprojects\femi>svcutil /language:C# /out:IQBWebConnService.cs /n:*,FCQBWebConnAPI QBWebConnectorSvc.wsdl
    //http://blogs.msdn.com/dotnetinterop/archive/2008/09/24/wsdl-first-development-with-wcf.aspx
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class QBWebConnSvcSoap : IQBWebConnSvcSoap
    {
        #region Consts
        const string StandardXMlHeader = "<?xml version = \"1.0\" encoding=\"utf-8\"?>";
        const string QBXMLHeader = "<?qbxml version=\"2.0\"?>";
        #endregion
        #region Global Vars
        private static readonly ILog log = LogManager.GetLogger(typeof(QBWebConnSvcSoap));
        eClatModel db;
        eClatModel _dml;
        public ArrayList req = new ArrayList();
        private int Maxretries = int.Parse(ConfigurationManager.AppSettings["MaxRetries"].ToString());
        //private string strRequestXML;
        CustomerAddRsModel.QBXMLMsgsRs qbMsgs;
        QBInvoiceAddRsModel.QBXMLInvMsgsRs qbInvMsgs;
        QBPaymentAddRsModel.QBXMLPaymentMsgsRs qbPayMsgs;
        QBCustomerQueryRsModel.QBCustQrXMLMsgsRs qbCustQMsgs;
        QBBillAddRsModel.QBXMLBillMsgsRs qbBillMsgs;
        #endregion

        #region Constructor
        public QBWebConnSvcSoap()
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
            try
            {
                //authReturn.Add( System.Guid.NewGuid().ToString());
                //DateTime.Now.ToString("HH:mm:ss.ffffff");



                string authUserName = ConfigurationManager.AppSettings["authUserName"].ToString();
                string authPassword = ConfigurationManager.AppSettings["authPassword"].ToString();

                log.Info(string.Format("Attempting to authenticate with user {0} ", authUserName));

                //log.Info(string.Format("request containts user {0} and pass {1}", request.Body.strUserName.Trim(), request.Body.strPassword.Trim()));

                //check if there is any work to do


                if (request.Body.strUserName.Trim().Equals(authUserName) && request.Body.strPassword.Trim().Equals(authPassword))
                {
                    log.Info("Authenticated with user :" + authUserName);
                    int count = db.qb_jobs.Where(a => a.job_status == "P" && a.retry_count < Maxretries).Count();
                    log.Info(string.Format(" {0} items pending ", count));
                    if (count < 1)
                    {
                        authReturn.Add(System.Guid.NewGuid().ToString());
                    }
                    else
                    {
                        string qbTicket = db.qb_jobs.Where(a => a.job_status == "P" && a.retry_count < Maxretries).OrderBy(x => x.jobid).Select(y => y.qb_ticket_id).FirstOrDefault();
                        authReturn.Add(qbTicket);
                    }

                    // An empty string for authReturn[1] means asking QBWebConnector 
                    // to connect to the company file that is currently openned in QB
                    string operationvalue = string.Empty;
                    operationvalue = count < 1 ? "NONE" : "";
                    authReturn.Add(operationvalue);
                    //authReturn.Add(ConfigurationManager.AppSettings["CompanyFileLocation"].ToString());
                    //"c:\\Program Files\\Intuit\\QuickBooks\\sample_product-based business.qbw";
                }
                else
                {
                    log.Info(string.Format(" Authentication failed for {0}", authUserName));
                    string operationvalue = "NVU";
                    authReturn.Add(operationvalue);
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in web service : " + ex.StackTrace);
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
            double recommendedVersion = double.Parse(ConfigurationManager.AppSettings["recommendedVersion"].ToString());
            double supportedMinVersion = double.Parse(ConfigurationManager.AppSettings["supportedMinVersion"].ToString());
            double suppliedVersion = Convert.ToDouble(this.parseForVersion(request.Body.strVersion));
            if (suppliedVersion < recommendedVersion)
            {
                retVal = "W:We recommend that you upgrade your QBWebConnector";
            }
            else if (suppliedVersion < supportedMinVersion)
            {
                retVal = "E:You need to upgrade your QBWebConnector";
            }
            if (suppliedVersion > recommendedVersion)
            {
                retVal = "O:1.5";
            }
            clientVersionResponse rsp = new clientVersionResponse(new clientVersionResponseBody(retVal));
            return rsp;
        }

        public closeConnectionResponse closeConnection(closeConnectionRequest request)
        {
            string evLogTxt = "WebMethod: closeConnection() has been called by QBWebconnector" + "\r\n\r\n";
            evLogTxt = evLogTxt + "Parameters received:\r\n";
            evLogTxt = evLogTxt + "string ticket = " + request.Body.ticket + "\r\n";
            evLogTxt = evLogTxt + "\r\n";
            string retVal = null;


            retVal = "OK";

            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string retVal= " + retVal + "\r\n";
            log.Info(evLogTxt);
            closeConnectionResponse rsp = new closeConnectionResponse(new closeConnectionResponseBody(retVal));
            return rsp;
        }
        /// <summary>
		/// WebMethod - connectionError()
		/// To facilitate capturing of QuickBooks error and notifying it to web services
		/// Signature: public string connectionError (string ticket, string hresult, string message)
		///
		/// IN: 
		/// string ticket = A GUID based ticket string to maintain identity of QBWebConnector 
		/// string hresult = An HRESULT value thrown by QuickBooks when trying to make connection
		/// string message = An error message corresponding to the HRESULT
		///
		/// OUT:
		/// string retVal
		/// Possible values: 
		/// - “done” = no further action required from QBWebConnector
		/// - any other string value = use this name for company file
		/// </summary>
        public connectionErrorResponse connectionError(connectionErrorRequest request)
        {
            //if (Session["ce_counter"] == null)
            //{
            //    Session["ce_counter"] = 0;
            //}
            SessionStateManager state = (SessionStateManager)HttpContext.Current.Session["AppSessionState"];
            if (state == null)
            {
                state = new SessionStateManager();
                HttpContext.Current.Session["AppSessionState"] = state;
            }

            string evLogTxt = "WebMethod: connectionError() has been called by QBWebconnector" + "\r\n\r\n";
            string retVal = null;
            // 0x80040400 - QuickBooks found an error when parsing the provided XML text stream. 
            const string QB_ERROR_WHEN_PARSING = "0x80040400";
            // 0x80040401 - Could not access QuickBooks.  
            const string QB_COULDNT_ACCESS_QB = "0x80040401";
            // 0x80040402 - Unexpected error. Check the qbsdklog.txt file for possible, additional information. 
            const string QB_UNEXPECTED_ERROR = "0x80040402";
            // Add more as you need...

            if (request.Body.hresult.Trim().Equals(QB_ERROR_WHEN_PARSING))
            {
                evLogTxt = evLogTxt + "HRESULT = " + request.Body.hresult + "\r\n";
                evLogTxt = evLogTxt + "Message = " + request.Body.message + "\r\n";

                retVal = "DONE";
            }
            else if (request.Body.hresult.Trim().Equals(QB_COULDNT_ACCESS_QB))
            {
                evLogTxt = evLogTxt + "HRESULT = " + request.Body.hresult + "\r\n";
                evLogTxt = evLogTxt + "Message = " + request.Body.message + "\r\n";

                retVal = "DONE";
            }
            else if (request.Body.hresult.Trim().Equals(QB_UNEXPECTED_ERROR))
            {
                evLogTxt = evLogTxt + "HRESULT = " + request.Body.hresult + "\r\n";
                evLogTxt = evLogTxt + "Message = " + request.Body.message + "\r\n";

                retVal = "DONE";
            }
            else
            {
                // Depending on various hresults return different value 
                if (state.Counter > 2)
                {
                    // Try again with this company file
                    evLogTxt = evLogTxt + "HRESULT = " + request.Body.hresult + "\r\n";
                    evLogTxt = evLogTxt + "Message = " + request.Body.message + "\r\n";
                    evLogTxt = evLogTxt + "Sending empty company file to try again.";
                    retVal = "";
                }
                else
                {
                    evLogTxt = evLogTxt + "HRESULT = " + request.Body.hresult + "\r\n";
                    evLogTxt = evLogTxt + "Message = " + request.Body.message + "\r\n";
                    evLogTxt = evLogTxt + "Sending DONE to stop.";
                    retVal = "DONE";
                }
            }
            saveLastError(0, request.Body.hresult + ":" + request.Body.message, request.Body.ticket);
            evLogTxt = evLogTxt + "\r\n";
            evLogTxt = evLogTxt + "Return values: " + "\r\n";
            evLogTxt = evLogTxt + "string retVal = " + retVal + "\r\n";
            log.Info(evLogTxt);
            int counter = state.Touch();

            log.Info(string.Format("[Session '{0}'] with request id : {1} (counter: {2})", HttpContext.Current.Session.SessionID, request.Body.ticket, counter));

            //Session["ce_counter"] = ((int)Session["ce_counter"]) + 1;
            connectionErrorResponse rsp = new connectionErrorResponse(new connectionErrorResponseBody(retVal));
            return rsp;
        }

        public getLastErrorResponse getLastError(getLastErrorRequest request)
        {
            int errorCode = 0;
            string retVal = null;
            //DateTime _today = DateTime.Now.Date;
            if (errorCode == -101)
            {
                retVal = "QuickBooks was not running!"; // This is just an example of custom user errors
            }
            else
            {
                retVal = db.errorlogs.Where(a => a.ticketid == request.Body.ticket).OrderByDescending(a => a.errordate).Select(a => a.lasterrormsg).FirstOrDefault();
                //getLastError(request.Body.ticket);
            }
            getLastErrorResponse rsp = new getLastErrorResponse(new getLastErrorResponseBody(retVal));
            return rsp;
        }
        /// <summary>
		/// WebMethod - receiveResponseXML()
		/// Signature: public int receiveResponseXML(string ticket, string response, string hresult, string message)
		/// 
		/// IN: 
		/// string ticket
		/// string response
		/// string hresult
		/// string message
		///
		/// OUT: 
		/// int retVal
		/// Greater than zero  = There are more request to send
		/// 100 = Done. no more request to send
		/// Less than zero  = Custom Error codes
		/// </summary>
        public receiveResponseXMLResponse receiveResponseXML(receiveResponseXMLRequest request)
        {
            //See responseCodes here :http://www.consolibyte.com/docs/index.php/QuickBooks_SDK_Error_Codes
            receiveResponseXMLResponse rsp;
            try
            {
                //SessionStateManager state = (SessionStateManager)HttpContext.Current.Session["AppSessionState"];
                //if (state == null)
                //{
                //    state = new SessionStateManager();
                //    HttpContext.Current.Session["AppSessionState"] = state;
                //}
                string evLogTxt = "WebMethod: receiveResponseXML() has been called by QBWebconnector" + "\r\n\r\n";
                evLogTxt = evLogTxt + "Parameters received:\r\n";
                evLogTxt = evLogTxt + "string ticket = " + request.Body.ticket + "\r\n";
                evLogTxt = evLogTxt + "string responseXML from QB = --+" + request.Body.response + "+--\r\n";
                evLogTxt = evLogTxt + "string hresult = " + request.Body.hresult + "\r\n";
                evLogTxt = evLogTxt + "string message = " + request.Body.message + "\r\n";
                evLogTxt = evLogTxt + "\r\n";

                int retVal = 0;
                if (!request.Body.hresult.ToString().Equals(""))
                {
                    // if there is an error with response received, web service could also return a -ve int		
                    evLogTxt = evLogTxt + "HRESULT = " + request.Body.hresult + "\r\n";
                    evLogTxt = evLogTxt + "Message = " + request.Body.message + "\r\n";
                    retVal = -101;
                    saveLastError(-101, request.Body.message, request.Body.ticket);
                }
                else
                {
                    //evLogTxt = evLogTxt + "Response received = --+" + request.Body.response + "+--\r\n";
                    log.Info(evLogTxt);
                    var currentResponse = db.qb_jobs.Where(a => a.qb_ticket_id == request.Body.ticket).FirstOrDefault();
                    switch (currentResponse.job_type)
                    {
                        #region CustomerAdd
                        case (int)JobType.CustomerAdd:
                            CustomerAddRsModel modl = new CustomerAddRsModel();
                            //temporary response file :
                            //StreamReader sr = new StreamReader(@"F:\LaoluOlapegba\Myprojects\femi\FCWBWebConnSvc\FCWBWebConnSvc\qbXML\CustomerAddRs.xml");
                            //get the deserilized response object
                            CustomerAddRsModel responseObject = modl.ReadCustomerAddRsXml(request.Body.response);

                            //close the temp file
                            //sr.Close();

                            qbMsgs = responseObject.QBMsgsRs;
                            CustomerAddRs rsAttributes = qbMsgs.CustAddRs;
                            CustomerAddRsViewModel custRetNode = rsAttributes.CustAddRs;
                            System.Text.StringBuilder logMessage = new System.Text.StringBuilder();
                            if (rsAttributes != null) // always true ?
                            {
                                //get the status Code, info and Severity
                                int retStatusCode = rsAttributes.statusCode;
                                string retStatusSeverity = rsAttributes.statusSeverity;
                                string retStatusMessage = rsAttributes.statusMessage;
                                logMessage.Append("CustomerAdd response codes :" + "\r\n\r\n");
                                logMessage.AppendFormat("statusCode = {0}, statusSeverity = {1}, statusMessage = {2}",
                                    retStatusCode, retStatusSeverity, retStatusMessage);
                                string qbrecordId = string.Empty;
                                if (custRetNode != null)
                                {
                                    //get the CustomerRet node for detailed info
                                    logMessage.AppendFormat("\r\nCustomer ListID = {0}", custRetNode.ListID);
                                    logMessage.AppendFormat("\r\nCustomer Name = {0}", custRetNode.Name);
                                    logMessage.AppendFormat("\r\nCustomer FullName = {0}", custRetNode.FullName);

                                    qbrecordId = custRetNode.ListID;
                                    evLogTxt = evLogTxt + logMessage;
                                }


                                //log.Info(logMessage);
                                //Treat
                                ///update the status of the transaction
                                ///
                                if (retStatusCode == 0)
                                {
                                    UpdateCustomerAddJob(request.Body.ticket, qbrecordId);
                                }
                                else
                                {
                                    saveLastError(retStatusCode, retStatusMessage, request.Body.ticket);
                                }
                                UpdateQJobStatus(request.Body.ticket, retStatusCode, retStatusMessage, qbrecordId);

                            }
                            break;
                        #endregion
                        #region InvoiceAdd
                        case (int)JobType.InvoiceAdd:
                            QBInvoiceAddRsModel rsModel = new QBInvoiceAddRsModel();
                            QBInvoiceAddRsModel invResponse = rsModel.ReadInvoiceAddRsXml(request.Body.response);

                            qbInvMsgs = invResponse.QBInvMsgsRs;
                            QBInvoiceAddRsModel.InvoiceAddRs rsAttribute = qbInvMsgs.InvoiceAddRs;
                            QBInvoiceAddRsModel.InvoiceRetModel invRetNode = rsAttribute.InvoiceAdd;
                            System.Text.StringBuilder logMsg = new System.Text.StringBuilder();
                            if (rsAttribute != null) // always true ?
                            {
                                //get the status Code, info and Severity
                                int retStatusCode = rsAttribute.statusCode;
                                string retStatusSeverity = rsAttribute.statusSeverity;
                                string retStatusMessage = rsAttribute.statusMessage;
                                logMsg.Append("InvoiceAdd response codes :" + "\r\n\r\n");
                                logMsg.AppendFormat("statusCode = {0}, statusSeverity = {1}, statusMessage = {2}",
                                    retStatusCode, retStatusSeverity, retStatusMessage);
                                string qbrecordId = string.Empty;
                                if (invRetNode != null)
                                {
                                    //get the CustomerRet node for detailed info
                                    logMsg.AppendFormat("\r\nCustomer ListID = {0}", invRetNode.CustomerNo.ListID);
                                    logMsg.AppendFormat("\r\nCustomer Name = {0}", invRetNode.CustomerNo.ListID);
                                    logMsg.AppendFormat("\r\nCustomer FullName = {0}", invRetNode.CustomerNo.ListID);

                                    qbrecordId = invRetNode.TxnID;
                                    evLogTxt = evLogTxt + logMsg;
                                }

                                //Treat
                                ///update the status of the transaction
                                ///
                                if (retStatusCode == 0)
                                {
                                    UpdateInvoiceAddJob(request.Body.ticket, qbrecordId);
                                }
                                else
                                {
                                    saveLastError(retStatusCode, retStatusMessage, request.Body.ticket);
                                }
                                UpdateQJobStatus(request.Body.ticket, retStatusCode, retStatusMessage, qbrecordId);

                            }
                            break;
                        #endregion
                        #region PaymentAdd
                        case (int)JobType.PaymentAdd:
                            QBPaymentAddRsModel rsMod = new QBPaymentAddRsModel();
                            QBPaymentAddRsModel payResponse = rsMod.ReadPaymentAddRsXml(request.Body.response);

                            qbPayMsgs = payResponse.QBPaymentMsgsRs;
                            QBPaymentAddRsModel.ReceivePaymentAddRs rsAttr = qbPayMsgs.ReceivePaymentAddRs;
                            QBPaymentAddRsModel.ReceivePaymentRet payRetNode = rsAttr.ReceivePaymentRet;
                            System.Text.StringBuilder lgMsg = new System.Text.StringBuilder();
                            if (rsAttr != null) // always true ?
                            {
                                //get the status Code, info and Severity
                                int retStatusCode = rsAttr.statusCode;
                                string retStatusSeverity = rsAttr.statusSeverity;
                                string retStatusMessage = rsAttr.statusMessage;
                                lgMsg.Append("PaymentAdd response codes :" + "\r\n\r\n");
                                lgMsg.AppendFormat("statusCode = {0}, statusSeverity = {1}, statusMessage = {2}",
                                    retStatusCode, retStatusSeverity, retStatusMessage);
                                string qbrecordId = string.Empty;
                                if (payRetNode != null)
                                {
                                    //get the CustomerRet node for detailed info
                                    lgMsg.AppendFormat("\r\nCustomer ListID = {0}", payRetNode.CustomerNo.ListID);
                                    lgMsg.AppendFormat("\r\nCustomer Name = {0}", payRetNode.CustomerNo.ListID);
                                    lgMsg.AppendFormat("\r\nCustomer FullName = {0}", payRetNode.CustomerNo.ListID);

                                    qbrecordId = payRetNode.TxnID;
                                    evLogTxt = evLogTxt + lgMsg;
                                }

                                //Treat
                                ///update the status of the transaction
                                ///
                                if (retStatusCode == 0)
                                {
                                    UpdatePaymentAddJob(request.Body.ticket, qbrecordId);
                                }
                                else
                                {
                                    saveLastError(retStatusCode, retStatusMessage, request.Body.ticket);
                                }
                                UpdateQJobStatus(request.Body.ticket, retStatusCode, retStatusMessage, qbrecordId);

                            }
                            break;
                        #endregion
                        #region CustomerQuery
                        case (int)JobType.CustomerQuery:
                            QBCustomerQueryRsModel custQrMod = new QBCustomerQueryRsModel();
                            QBCustomerQueryRsModel queryResponse = custQrMod.ReadCustomerQueryRsXml(request.Body.response);

                            qbCustQMsgs = queryResponse.QBMsgsRs;
                            QBCustomerQueryRsModel.CustomerQueryRs custRrsAttr = qbCustQMsgs.CustQueryRs;
                            QBCustomerQueryRsModel.CustomerQueryRet[] custQueryRetNode = custRrsAttr.CustomerRet;
                            System.Text.StringBuilder CqMsg = new System.Text.StringBuilder();
                            if (custRrsAttr != null) // always true ?
                            {
                                //get the status Code, info and Severity
                                int retStatusCode = custRrsAttr.statusCode;
                                //string retStatusSeverity = custRrsAttr.;
                                string retStatusMessage = custRrsAttr.statusMessage;

                                CqMsg.Append("CustomerQuery response codes :" + "\r\n\r\n");
                                CqMsg.AppendFormat("statusCode = {0}, statusMessage = {1}, remainining = {2}",
                                    retStatusCode, retStatusMessage, custRrsAttr.iteratorRemainingCount);
                                string qbrecordId = string.Empty;
                                string iteratorid = string.Empty;
                                if(custRrsAttr.iteratorID != null)
                                    iteratorid = custRrsAttr.iteratorID;
                                int remaining = custRrsAttr.iteratorRemainingCount; 
                                
                                if (retStatusCode == 0)
                                {
                                    //while (custRrsAttr.iteratorRemainingCount > 0 )
                                    //{
                                    if (custQueryRetNode != null)
                                    {
                                        //get the CustomerRet node for detailed info
                                        foreach (var item in custQueryRetNode)
                                        {
                                            //insert into DB
                                            qb_customers entity = new qb_customers();
                                            entity.Name = item.Name;
                                            entity.ListID = item.ListID;
                                            entity.IsActive = item.IsActive;
                                            entity.FullName = item.FullName;
                                            entity.Balance = item.Balance;
                                            entity.EditSequence = item.EditSequence;
                                            entity.Sublevel = item.Sublevel;
                                            entity.TimeCreated = DateTime.ParseExact(item.TimeCreated, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);

                                            entity.TimeModified = DateTime.ParseExact(item.TimeModified, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);
                                            //Convert.ToDateTime(item.TimeModified);
                                            entity.TotalBalance = item.TotalBalance;

                                            db.qb_customers.Add(entity);
                                        }
                                        db.SaveChanges();

                                        qbrecordId = "-";// custRetRetNode.TxnID;
                                        evLogTxt = evLogTxt + CqMsg;
                                        log.Info(evLogTxt);
                                        evLogTxt = string.Empty;
                                    }
                                    log.Info("updating job status for ticket id :" + request.Body.ticket);
                                    UpdateQJobStatus(request.Body.ticket, retStatusCode, retStatusMessage, qbrecordId);
                                    log.Info("updated job status for ticket id :" + request.Body.ticket);
                                    //if more items left, log the next one
                                    if (remaining > 0)
                                    {
                                        qb_jobs jobentity = new qb_jobs();
                                        jobentity.error_code = "0";
                                        jobentity.FromDate = currentResponse.FromDate;
                                        jobentity.job_type = currentResponse.job_type;
                                        jobentity.MaxReturn = currentResponse.MaxReturn;
                                        jobentity.trans_id = "0";
                                        jobentity.job_status = "P";
                                        string new_qb_ticket_id = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                                        jobentity.qb_ticket_id = new_qb_ticket_id;
                                        jobentity.retry_count = 0;
                                        jobentity.iteratorid = iteratorid;
                                        jobentity.iterator = "Continue";
                                        jobentity.ext_sys_id = "-";
                                        jobentity.ext_sys_token = "-";
                                        db.qb_jobs.Add(jobentity);
                                        db.SaveChanges();
                                        log.Info("added new ticket id :" + new_qb_ticket_id);

                                        //send d request to QB manually
                                        //sendRequestXMLRequest newreq = new sendRequestXMLRequest();
                                        //sendRequestXMLRequestBody rqstBody = new sendRequestXMLRequestBody();
                                        //rqstBody.qbXMLCountry = "US";
                                        //rqstBody.qbXMLMajorVers = 13;
                                        //rqstBody.qbXMLMinorVers = 0;
                                        //rqstBody.strCompanyFileName = @"F:\QB ENT 2012\ECLINIC TEST.QBW";
                                        //rqstBody.strHCPResponse = "--truncated--";
                                        //rqstBody.ticket = new_qb_ticket_id;
                                        
                                        //newreq.Body = rqstBody;
                                        //sendRequestXMLResponse newresponse = sendRequestXML(newreq);
                                        //log.Info("New Rsp :" + newresponse.Body.sendRequestXMLResult);

                                    }
                                    //}

                                }
                                

                            }
                            break;
                        #endregion
                        #region BillAdd_Supplier
                        case (int)JobType.BillAdd:
                            QBBillAddRsModel rsNod = new QBBillAddRsModel();
                            QBBillAddRsModel billResponse = rsNod.ReadBillAddRsXml(request.Body.response);

                            qbBillMsgs = billResponse.QBBillMsgsRs;
                            QBBillAddRsModel.BillAddRs billrsAttr = qbBillMsgs.BillAddRs;
                            QBBillAddRsModel.BillRet billRetNode = billrsAttr.BillAddRet;
                            System.Text.StringBuilder billgMsg = new System.Text.StringBuilder();
                            if (billrsAttr != null) // always true ?
                            {
                                //get the status Code, info and Severity
                                int retStatusCode = billrsAttr.statusCode;
                                string retStatusSeverity = billrsAttr.statusSeverity;
                                string retStatusMessage = billrsAttr.statusMessage;
                                billgMsg.Append("PaymentAdd response codes :" + "\r\n\r\n");
                                billgMsg.AppendFormat("statusCode = {0}, statusSeverity = {1}, statusMessage = {2}",
                                    retStatusCode, retStatusSeverity, retStatusMessage);
                                string qbrecordId = string.Empty;
                                if (billRetNode != null)
                                {
                                    //get the CustomerRet node for detailed info
                                    billgMsg.AppendFormat("\r\n VendorNo ListID = {0}", billRetNode.VendorNo.ListID);
                                    billgMsg.AppendFormat("\r\n VendorNo Name = {0}", billRetNode.VendorNo.ListID);
                                    billgMsg.AppendFormat("\r\n VendorNo FullName = {0}", billRetNode.VendorNo.ListID);

                                    qbrecordId = billRetNode.TxnID;
                                    evLogTxt = evLogTxt + billgMsg;
                                }

                                //Treat
                                ///update the status of the transaction
                                ///
                                if (retStatusCode == 0)
                                {
                                    UpdatePaymentAddJob(request.Body.ticket, qbrecordId);
                                }
                                else
                                {
                                    saveLastError(retStatusCode, retStatusMessage, request.Body.ticket);
                                }
                                UpdateQJobStatus(request.Body.ticket, retStatusCode, retStatusMessage, qbrecordId);

                            }
                            break;
                            #endregion
                    }
                }

                //ArrayList req = buildRequest();
                //int pendingCount = db.qb_jobs.Where(a => a.job_status == "P").Count();
                int total = db.qb_jobs.Where(a => a.job_status == "P").Count(); //req.Count;
                int count = Convert.ToInt32(HttpContext.Current.Session["counter"]);
                if (total > 0)
                {
                    int percentage = (count * 100) / total;
                    if (percentage >= 100)
                    {
                        count = 0;
                        HttpContext.Current.Session["counter"] = 0;
                    }
                    retVal = percentage;
                }
                else
                {
                    HttpContext.Current.Session["counter"] = 0;
                }

                evLogTxt = evLogTxt + "\r\n";
                evLogTxt = evLogTxt + "Return values: " + "\r\n";
                evLogTxt = evLogTxt + "int retVal= " + retVal.ToString() + "\r\n";
                log.Info(evLogTxt);

                rsp = new receiveResponseXMLResponse(new receiveResponseXMLResponseBody(retVal));
            }
            catch (Exception ex)
            {
                //set responseBody = -ve number so QBCOnn can call getlastError
                rsp = new receiveResponseXMLResponse(new receiveResponseXMLResponseBody(-1));
                log.Info("Error in receiveResponseXML :" + ex.StackTrace);
                log.Error("", ex);
            }
            return rsp;
        }

        private void UpdatePaymentAddJob(string ticketId, string qbrecordId)
        {
        //    //var transaction = db.patients.Find(ticketId);
        //    var transaction = db.qb_jobs.Where(a => a.qb_ticket_id == ticketId).FirstOrDefault();
        //    if (transaction == null)
        //    {
        //        log.Error(string.Format("Cannot update cust alloc record with trans no:{0} as it's not available.", ticketId));
        //    }
        //    else
        //    {
        //        int txnId = int.Parse(transaction.trans_id);
        //        var inventity = db.cust_allocations.Where(a => a.id == txnId).FirstOrDefault();
        //        inventity.ext_sync_date = DateTime.Now;
        //        inventity.ext_sync_flag = true;
        //        inventity.ext_sys_id = qbrecordId;
        //        db.cust_allocations.Attach(inventity);

        //        db.Entry(inventity).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
        //    }
        }
        private void UpdateInvoiceAddJob(string ticketId, string qbrecordId)
        {
            ////var transaction = db.patients.Find(ticketId);
            //var transaction = db.qb_jobs.Where(a => a.qb_ticket_id == ticketId).FirstOrDefault();
            //if (transaction == null)
            //{
            //    log.Error(string.Format("Cannot update debtors trans record with trans no:{0} as it's not available.", ticketId));
            //}
            //else
            //{
            //    int txnId = int.Parse(transaction.trans_id);
            //    var inventity = db.debtor_trans.Where(a => a.trans_no == txnId).FirstOrDefault();
            //    inventity.ext_sync_date = DateTime.Now;
            //    inventity.ext_sync_flag = true;
            //    inventity.ext_sys_id = qbrecordId;
            //    db.debtor_trans.Attach(inventity);

            //    db.Entry(inventity).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}
        }

        private void UpdateCustomerAddJob(string ticketId, string qbListId)
        {

            //var transaction = db.qb_jobs.Where(a => a.qb_ticket_id == ticketId).FirstOrDefault();
            //if (transaction == null)
            //{
            //    log.Error(string.Format("Cannot update cust add record with trans no:{0} as it's not available.", ticketId));
            //}
            //else
            //{
            //    //int patientId = int.Parse(transaction.trans_id);
            //    string patientId = transaction.trans_id;
            //    var custentity = db.patients.Where(a => a.patient_id == patientId).FirstOrDefault();
            //    custentity.ext_sync_date = DateTime.Now;
            //    custentity.ext_sync_flag = true;
            //    custentity.ext_sys_id = qbListId;
            //    db.patients.Attach(custentity);

            //    db.Entry(custentity).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}
            try
            {
                var transaction = db.qb_jobs.Where(a => a.qb_ticket_id == ticketId).FirstOrDefault();
                string patientId = transaction.trans_id;
                var patient = db.patients.Where(a => a.patient_id == patientId).FirstOrDefault();

                qb_customers entity = new qb_customers();
                entity.Name = patient.surname + " " + patient.middle_name + " " + patient.forename;
                entity.ListID = qbListId;
                entity.IsActive = "1";
                entity.FullName = patient.surname;
                entity.Balance = "0";
                entity.EditSequence = "";
                entity.TimeCreated = DateTime.Now;
                entity.Firstname = patient.forename;
                entity.Middlename = patient.middle_name;
                entity.LastName = patient.surname;
                entity.upi = patient.upi;
                entity.TotalBalance = "";

                db.qb_customers.Add(entity);

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
                //DateTime.ParseExact(item.TimeCreated, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);


                //DateTime.ParseExact(item.TimeModified, "yyyy-MM-dd'T'HH:mm:sszzz", CultureInfo.InvariantCulture);
                //Convert.ToDateTime(item.TimeModified);

        }

        private void UpdateBillAddJob(string ticketId, string qbrecordId)
        {
            //var transaction = db.qb_jobs.Where(a => a.qb_ticket_id == ticketId).FirstOrDefault();
            //if (transaction == null)
            //{
            //    log.Error(string.Format("Cannot update bill add record with trans no:{0} as it's not available.", ticketId));
            //}
            //else
            //{
            //    int txnId = int.Parse(transaction.trans_id);
            //    var inventity = db.cust_allocations.Where(a => a.id == txnId).FirstOrDefault();
            //    inventity.ext_sync_date = DateTime.Now;
            //    inventity.ext_sync_flag = true;
            //    inventity.ext_sys_id = qbrecordId;
            //    db.cust_allocations.Attach(inventity);

            //    db.Entry(inventity).State = System.Data.Entity.EntityState.Modified;
            //    db.SaveChanges();
            //}
        }
        private void UpdateQJobStatus(string ticketId, int statusCode, string statusDesc, string qbIdentifier)
        {
            try
            {
                _dml = new eClatModel();
                var transaction = _dml.qb_jobs.Where(a => a.qb_ticket_id == ticketId).FirstOrDefault();
                if (transaction == null)
                {
                    log.Error(string.Format("Cannot update job with ticket no:{0} as it's not available.", ticketId));
                }
                else
                {
                    transaction.error_code = statusCode.ToString();
                    transaction.error_description = statusDesc;
                    if (statusCode == 0)
                    {
                        transaction.ext_sync_date = DateTime.Now;
                        transaction.job_status = "C";
                        transaction.ext_sys_id = qbIdentifier;

                    }
                    else
                    {
                        transaction.retry_count = transaction.retry_count + 1;
                    }
                    _dml.qb_jobs.Attach(transaction);
                    _dml.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    _dml.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }

        }
        private void UpdateQJobStatusAs(string ticketId, int statusCode, string statusDesc, string qbIdentifier)
        {
            try
            {
                _dml = new eClatModel();
                var transaction = _dml.qb_jobs.Where(a => a.qb_ticket_id == ticketId).FirstOrDefault();
                if (transaction == null)
                {
                    log.Error(string.Format("Cannot update job with ticket no:{0} as it's not available.", ticketId));
                }
                else
                {
                    transaction.error_code = statusCode.ToString();
                    transaction.error_description = statusDesc;
                    transaction.job_status = "C";
                    //context.Entry(local).State = EntityState.Detached;
                    _dml.qb_jobs.Attach(transaction);
                    _dml.Entry(transaction).State = System.Data.Entity.EntityState.Modified;
                    _dml.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
            }
        }

        /// <summary>
        /// WebMethod - sendRequestXML()
        /// Signature: public string sendRequestXML(string ticket, string strHCPResponse, string strCompanyFileName, 
        /// string Country, int qbXMLMajorVers, int qbXMLMinorVers)
        /// 
        /// IN: 
        /// int qbXMLMajorVers
        /// int qbXMLMinorVers
        /// string ticket
        /// string strHCPResponse 
        /// string strCompanyFileName 
        /// string Country
        /// int qbXMLMajorVers
        /// int qbXMLMinorVers
        ///
        /// OUT:
        /// string request
        /// Possible values: 
        /// - “any_string” = Request XML for QBWebConnector to process
        /// - "" = No more request XML 
        /// </summary>
        public sendRequestXMLResponse sendRequestXML(sendRequestXMLRequest request)
        {
            sendRequestXMLResponse rsp;
            try
            {
                SessionStateManager state = (SessionStateManager)HttpContext.Current.Session["AppSessionState"];
                if (state == null)
                {
                    state = new SessionStateManager();
                    HttpContext.Current.Session["AppSessionState"] = state;
                }
                string evLogTxt = "WebMethod: sendRequestXML() has been called by QBWebconnector" + "\r\n\r\n";
                evLogTxt = evLogTxt + "Parameters received:\r\n";
                evLogTxt = evLogTxt + "string ticket = " + request.Body.ticket + "\r\n";
                //evLogTxt = evLogTxt + "string strHCPResponse = --+" + request.Body.strHCPResponse + "+--\r\n";
                evLogTxt = evLogTxt + "string strCompanyFileName = " + request.Body.strCompanyFileName + "\r\n";
                evLogTxt = evLogTxt + "string qbXMLCountry = " + request.Body.qbXMLCountry + "\r\n";
                evLogTxt = evLogTxt + "int qbXMLMajorVers = " + request.Body.qbXMLMajorVers.ToString() + "\r\n";
                evLogTxt = evLogTxt + "int qbXMLMinorVers = " + request.Body.qbXMLMinorVers.ToString() + "\r\n";
                evLogTxt = evLogTxt + "\r\n";


                string _request = "";
                evLogTxt = evLogTxt + "getting pending requests for ticket id:" + request.Body.ticket + "\r\n";
                log.Info(evLogTxt);
                evLogTxt = "";

                ArrayList req = getpendingRequests(request.Body.ticket.Trim());

                evLogTxt = evLogTxt + " arraylst count:" + req.Count + "\r\n";

                evLogTxt = evLogTxt + " RequestXML to QB = --+" + _request + "+-- \r\n";
                log.Info(evLogTxt);
                evLogTxt = "";
                int total = req.Count;

                int count = state.Counter;

                if (count < total)
                {
                    _request = req[count].ToString();
                    evLogTxt = evLogTxt + "sending request no = " + (count + 1) + "\r\n";
                    //Session["counter"] = ((int)Session["counter"]) + 1;
                    int counter = state.Touch();
                    log.Info(string.Format("[Session '{0}'] with ticket id : {1} (counter: {2})",
                        HttpContext.Current.Session.SessionID, request.Body.ticket, counter));

                }
                else
                {
                    count = 0;
                    //Session["counter"] = 0;
                    state.Counter = 0;
                    _request = "";
                }
                evLogTxt = evLogTxt + "\r\n";
                //evLogTxt = evLogTxt + "Return values: " + "\r\n";
                evLogTxt = evLogTxt + " RequestXML to QB = --+" + _request + "+-- \r\n";
                log.Info(evLogTxt);
                rsp = new sendRequestXMLResponse(new sendRequestXMLResponseBody(_request));
            }
            catch (Exception ex)
            {
                //set responseBody = string.empty so QBCOnn can call getlastError
                rsp = new sendRequestXMLResponse(new sendRequestXMLResponseBody(""));
                //saveLastError(ex, request.Body.ticket);
                log.Error("", ex);
            }

            return rsp;
        }

        public serverVersionResponse serverVersion(serverVersionRequest request)
        {
            string serverVersion = ConfigurationManager.AppSettings["supportedServerVersion"];
            serverVersionResponse rsp = new serverVersionResponse();
            rsp.Body = new serverVersionResponseBody(serverVersion);
            return rsp;
        }
        #endregion

        #region SupportMethods
        public ArrayList buildRequest()
        {
            string strRequestXML = "";
            XmlDocument inputXMLDoc = null;

            // CustomerQuery
            inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            XmlElement qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);
            XmlElement qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement customerQueryRq = inputXMLDoc.CreateElement("CustomerQueryRq");
            qbXMLMsgsRq.AppendChild(customerQueryRq);
            customerQueryRq.SetAttribute("requestID", "1");
            XmlElement maxReturned = inputXMLDoc.CreateElement("MaxReturned");
            customerQueryRq.AppendChild(maxReturned).InnerText = "1";

            strRequestXML = inputXMLDoc.OuterXml;
            req.Add(strRequestXML);

            // Clean up
            strRequestXML = "";
            inputXMLDoc = null;
            qbXML = null;
            qbXMLMsgsRq = null;
            maxReturned = null;

            // InvoiceQuery
            inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);
            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement invoiceQueryRq = inputXMLDoc.CreateElement("InvoiceQueryRq");
            qbXMLMsgsRq.AppendChild(invoiceQueryRq);
            invoiceQueryRq.SetAttribute("requestID", "2");
            maxReturned = inputXMLDoc.CreateElement("MaxReturned");
            invoiceQueryRq.AppendChild(maxReturned).InnerText = "1";

            strRequestXML = inputXMLDoc.OuterXml;
            req.Add(strRequestXML);

            // Clean up
            strRequestXML = "";
            inputXMLDoc = null;
            qbXML = null;
            qbXMLMsgsRq = null;
            maxReturned = null;

            // BillQuery
            inputXMLDoc = new XmlDocument();
            inputXMLDoc.AppendChild(inputXMLDoc.CreateXmlDeclaration("1.0", null, null));
            inputXMLDoc.AppendChild(inputXMLDoc.CreateProcessingInstruction("qbxml", "version=\"4.0\""));

            qbXML = inputXMLDoc.CreateElement("QBXML");
            inputXMLDoc.AppendChild(qbXML);
            qbXMLMsgsRq = inputXMLDoc.CreateElement("QBXMLMsgsRq");
            qbXML.AppendChild(qbXMLMsgsRq);
            qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
            XmlElement billQueryRq = inputXMLDoc.CreateElement("BillQueryRq");
            qbXMLMsgsRq.AppendChild(billQueryRq);
            billQueryRq.SetAttribute("requestID", "3");
            maxReturned = inputXMLDoc.CreateElement("MaxReturned");
            billQueryRq.AppendChild(maxReturned).InnerText = "1";

            strRequestXML = inputXMLDoc.OuterXml;
            req.Add(strRequestXML);

            return req;
        }
        public ArrayList getpendingRequests(string ticketId)
        {
            //log.Info(string.Format("inside getpendingRequests ticketid  {0}", ticketId));
            log.Info("inside getpendingRequests ticketid with ticket id = " + ticketId);
            string strRequestXML = "";
            string QBXML = "<QBXML>";
            string qbXMLHeader = "<?qbxml version=\"5.0\"?><QBXML>";
            ResponseObject rspObj;
            try
            {
                int pendingCount = db.qb_jobs.Where(a => a.job_status == "P").Where(a => a.qb_ticket_id == ticketId.Trim()).Count();
                log.Info("count of ticket = " + pendingCount);
                if (pendingCount > 0)
                {
                    var pendingjobs = db.qb_jobs.Where(a => a.job_status == "P").Where(a => a.qb_ticket_id == ticketId.Trim());
                    foreach (var item in pendingjobs)
                    {
                        if (item.retry_count < Maxretries)
                        {
                            switch (item.job_type)
                            {
                                case (int)JobType.CustomerAdd:
                                    CustomerAddModel modl = new CustomerAddModel();
                                    strRequestXML = modl.WriteCustomerAddXml(item.trans_id, item.jobid.ToString());

                                    if (strRequestXML != string.Empty)
                                    {
                                        strRequestXML = strRequestXML.Replace(QBXML, qbXMLHeader);
                                        req.Add(strRequestXML);
                                    }
                                    break;
                                case (int)JobType.InvoiceAdd:
                                    QBInvoiceAddRqModel invmodl = new QBInvoiceAddRqModel();

                                    //strRequestXML = 
                                    rspObj = invmodl.WriteInvoiceAddXml(int.Parse(item.trans_id), item.jobid.ToString());
                                    if (rspObj.statusCode == 0)
                                    {
                                        strRequestXML = rspObj.statusXML;
                                        if (strRequestXML != string.Empty)
                                        {
                                            strRequestXML = strRequestXML.Replace(QBXML, qbXMLHeader);
                                            req.Add(strRequestXML);
                                        }
                                    }
                                    else
                                    {
                                        UpdateQJobStatusAs(ticketId, rspObj.statusCode, rspObj.statusDesc, "");
                                    }
                                    break;
                                case (int)JobType.PaymentAdd:
                                    QBPaymentAddRqModel paymodl = new QBPaymentAddRqModel();
                                    //strRequestXML = paymodl.WritePaymentAddXml(int.Parse(item.trans_id), item.jobid.ToString());
                                    //if (strRequestXML != string.Empty)
                                    //{
                                    //    strRequestXML = strRequestXML.Replace(QBXML, qbXMLHeader);
                                    //    req.Add(strRequestXML);
                                    //}
                                    rspObj = paymodl.WritePaymentAddXml(int.Parse(item.trans_id), item.jobid.ToString());
                                    if (rspObj.statusCode == 0 )
                                    {
                                        strRequestXML = rspObj.statusXML;
                                        if (strRequestXML != string.Empty)
                                        {
                                            strRequestXML = strRequestXML.Replace(QBXML, qbXMLHeader);
                                            req.Add(strRequestXML);
                                        }
                                    }
                                    else
                                    {
                                        
                                        UpdateQJobStatusAs(ticketId, rspObj.statusCode, rspObj.statusDesc, "");
                                    }
                                    break;
                                case (int)JobType.CustomerQuery:
                                    QBCustomerQueryRqModel qurmodl = new QBCustomerQueryRqModel();
                                    log.Info(string.Format("calling writecustqueryxml with {0} , {1}, {2} , {3}, {4} , {5}:",
                                        item.jobid, item.FromDate, item.iterator, item.MaxReturn, item.iteratorid, item.fromname));
                                    strRequestXML = qurmodl.WriteCustomerQueryXml(item.jobid.ToString(), item.FromDate,
                                        item.MaxReturn, item.iterator, item.iteratorid, item.fromname, item.toname);
                                    log.Info(string.Format("returned XML with {0} ", strRequestXML));
                                    if (strRequestXML != string.Empty)
                                    {
                                        strRequestXML = strRequestXML.Replace(QBXML, qbXMLHeader);
                                        req.Add(strRequestXML);
                                    }
                                    break;
                                case (int)JobType.BillAdd:
                                    QBBillAddRqModel billmodl = new QBBillAddRqModel();
                                    log.Info(string.Format("calling WriteBillAddXml with {0} , {1} :",
                                        item.jobid, item.trans_id));
                                    strRequestXML = billmodl.WriteBillAddXml(long.Parse(item.trans_id), item.jobid.ToString());
                                    log.Info(string.Format("returned XML with {0} ", strRequestXML));
                                    if (strRequestXML != string.Empty)
                                    {
                                        strRequestXML = strRequestXML.Replace(QBXML, qbXMLHeader);
                                        req.Add(strRequestXML);
                                    }
                                    break;
                            }

                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error("Error occured in web service : " + ex.StackTrace);
            }

            return req;

        }

        private void HandleException(int statusCode, JobType jobType, string patientId)
        {
        //    switch (statusCode)
        //    {
        //        case 1:
        //            addJob(jobType, patientId);
        //            break;
        //        default:
        //            break;
        //    }
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
        private void saveLastError(int errorCode, string errorMsg, string ticketid)
        {
            errorlog log = new errorlog();
            log.lasterrormsg = errorMsg;
            log.lasterrorcode = errorCode.ToString();
            log.ticketid = ticketid;
            log.errordate = DateTime.Now;
            db.errorlogs.Add(log);
            db.SaveChanges();
        }


        private void doCustomerQuery(string jobId, string fromDate, int MaxReturn, string iterator)
        {
            //    QBCustomerQueryRqModel qurmodl = new QBCustomerQueryRqModel();
            //    string  strRequestXML = qurmodl.WriteCustomerQueryXml(jobId, fromDate, MaxReturn, iterator);
            //    if (strRequestXML != string.Empty)
            //    {
            //        //strRequestXML = strRequestXML.Replace(QBXML, qbXMLHeader);
            //        //req.Add(strRequestXML);
            //    }
        }
        
        
        #endregion

        //http://10.160.174.64:28880/epincertservice.asmx?wsdl   : http://10.160.174.64/EpinPubServices.asmx?wsdl
        //  svcutil /l:C# http://10.160.174.64:28880/epincertservice.asmx?wsdl  /config:Client.exe.config /targetClientVersion:Version35
        //  /n:*,PubCertSvc /ser:XmlSerializer /useSerializerForFaults
        //  csc /t:library /out:PubCertSvc.dll /r:System.dll /r:System.Xml.dll /r:System.Web.Services.dll EpinPubServices.cs

        //If the value of the tag is empty, then do not send that tag. i.e. rather than send an empty tag like: <LastName></LastName> just don't send that tag at all.
        //https://stackoverflow.com/questions/36937442/send-request-to-web-connector-using-my-application-quick-books
    }
}
