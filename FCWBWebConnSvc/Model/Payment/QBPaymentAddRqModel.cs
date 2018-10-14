using FCQB.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model.Payment
{

    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBPaymentAddRqModel
    {
        [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        public QBXMLPaymentMsgsRq QBMsgsRq;
        eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(QBPaymentAddRqModel));
        public ResponseObject WritePaymentAddXml(long InvoiceId, string jobId)
        {
            db = new eClatModel();
            string strQXML = string.Empty;
            ResponseObject rspObj = new ResponseObject();
            try
            {
                var data = (from inv in db.debtor_trans
                            join p in db.patients on inv.debtor_no equals p.upi
                            where inv.trans_no == InvoiceId
                            select new
                            {
                                upi = p.upi,
                                patientId = p.patient_id
                            }).FirstOrDefault();
                if (data == null)
                {
                    string errorDesc = string.Format("PaymentAdd: cannot find trans_no or patient UPI for trans_id {0} returning empty XML", InvoiceId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }
                int custcount = db.qb_customers.Where(a => a.upi == data.upi).Count();
                if(custcount < 1)
                {
                    //Create the customer
                    JobHelper hlp = new JobHelper();
                    hlp.addJob(Services.JobType.CustomerAdd, data.patientId);
                    rspObj.statusCode = 2;
                    string errorDesc = string.Format("PaymentAdd: cannot find upi {0} in qb_customer. new customer job submitted. returning empty XML", data.upi);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    return rspObj;

                }

                 var query = (from e in db.debtor_trans
                             join inv in db.patients on e.debtor_no equals inv.upi
                             join qb in db.qb_customers on inv.upi equals qb.upi
                             where e.trans_no == InvoiceId
                             //where e.sync_flag == false
                             select new
                             {
                                 PONumber = e.trans_no,  //.ToString()  //laolu should change to transno
                                 RefNumber = e.reference,
                                 Memo = e.clinic_token,
                                 TxnDate = e.tran_date,                      //.ToString("yyyy-mm-dd")         
                                 DebtorNo = qb.ListID,
                                 TotalAmount = e.ov_amount
                             }).ToList();
                if (query.Count < 1)
                {
                    string errorDesc = string.Format("PaymentAdd: cannot find trans and ListID with transid  {0} in eclinic DB. returning empty XML", InvoiceId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 3;
                    return rspObj;
                }
                var tmpinvoice = query.Select(e => new TmpPayAddModel
                {
                    PONumber = e.PONumber,  //.ToString()
                    RefNumber = e.RefNumber,
                    Memo = e.Memo,
                    TxnDate = e.TxnDate.ToString("yyyy-MM-dd"),                     //.ToString("yyyy-mm-dd")  
                    DebtorNo = e.DebtorNo,
                    TotalAmount = e.TotalAmount
                }).FirstOrDefault();
                PaymentAddModel invoice = new PaymentAddModel();                
                invoice.RefNumber = tmpinvoice.PONumber.ToString();
                invoice.Memo = tmpinvoice.Memo;
                invoice.TxnDate = tmpinvoice.TxnDate;
                invoice.TotalAmount = string.Format("{0:0.00}", tmpinvoice.TotalAmount); //tmpinvoice.TotalAmount;
                invoice.IsAutoApply = "true";


                PaymentMethodRef paymethod = new PaymentMethodRef();
                paymethod.FullName = "Cash";

                invoice.PaymentMethodRef = paymethod;
                CustomerRef custref = new CustomerRef();
                log.Info("Log debtorNo =" + tmpinvoice.DebtorNo);
                custref.ListID = tmpinvoice.DebtorNo;


                invoice.CustomerNo = custref;

                if (invoice == null )
                {
                    rspObj.statusXML = string.Empty;
                    rspObj.statusDesc = " payment details not found ";
                    rspObj.statusCode = 3;
                    return rspObj;
                }

                PaymentAddRq PayAddreq = new PaymentAddRq();
                PayAddreq.requestID = jobId;
                string debtorNo = invoice.CustomerNo.ListID;

                invoice.CustomerNo = custref;

                //invoice.InvoiceLineItems = itemdetails.ToList();


                PayAddreq.PaymentAdd = invoice;

                QBXMLPaymentMsgsRq qbMsgReqs = new QBXMLPaymentMsgsRq();
                qbMsgReqs.PaymentAddRq = PayAddreq;


                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer serializer =
                new XmlSerializer(typeof(QBPaymentAddRqModel));

                QBPaymentAddRqModel modl = new QBPaymentAddRqModel();
                modl.QBMsgsRq = qbMsgReqs;
                //StringWriter writer1 = new StringWriter();  //@"c:\temp\qbXml.xml"
                // Serialize the model, and close the TextWriter.
                using (StringWriter writer = new Utf8StringWriter())
                {
                    serializer.Serialize(writer, modl, ns);
                    strQXML = writer.ToString();
                }
                rspObj.statusXML = strQXML;
            }
            catch (Exception ex)
            {
                rspObj.statusDesc = ex.Message;
                rspObj.statusCode = 99;
                log.Error(ex.StackTrace, ex);
            }

            return rspObj;
        }

    }
    public class QBXMLPaymentMsgsRq
    {

        [XmlElementAttribute("ReceivePaymentAddRq", IsNullable = false)]
        public PaymentAddRq PaymentAddRq;
        [XmlAttribute(AttributeName = "onError")]
        public string onError { get; set; }
        public QBXMLPaymentMsgsRq()
        {
            onError = "stopOnError";
        }
    }

    public class PaymentAddRq
    {
        [XmlElementAttribute("ReceivePaymentAdd", IsNullable = false)]
        public PaymentAddModel PaymentAdd;
        [XmlAttribute(AttributeName = "requestID")]
        public string requestID { get; set; }
    }

    public class PaymentAddModel
    {

        [XmlElementAttribute("CustomerRef", Order = 1)]
        public CustomerRef CustomerNo { get; set; }
        [XmlElementAttribute("TxnDate", Order = 2)]
        public string TxnDate;
        [XmlElementAttribute("RefNumber", Order = 3)]
        public string RefNumber { get; set; }
        [XmlElementAttribute("TotalAmount", Order = 4)]
        public string TotalAmount;
        [XmlElementAttribute("PaymentMethodRef", Order = 5)]
        public PaymentMethodRef PaymentMethodRef { get; set; }
        [XmlElementAttribute("Memo", Order = 6)]
        public string Memo { get; set; }

        [XmlElementAttribute("AppliedToTxnAdd", Order = 7)]
        public AppliedToTxnAdd AppliedToTxnAdd { get; set; }
        [XmlElementAttribute("IsAutoApply", Order = 8)]
        public string IsAutoApply { get; set; }

    }

    public class PaymentMethodRef
    {
        [XmlElementAttribute("FullName", Order = 1)]
        public string FullName { get; set; }

    }
    public class AppliedToTxnAdd
    {
        [XmlElementAttribute("TxnID", Order = 1)]
        public string TxnID { get; set; }
        [XmlElementAttribute("PaymentAmount", Order = 2)]
        public decimal PaymentAmount { get; set; }

    }

}