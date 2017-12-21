using FCQB.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using static FCQBWebConnAPI.Model.CustomerAddModel;

namespace FCQBWebConnAPI.Model
{

    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBInvoiceAddRqModel
    {
        [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        public QBXMLMsgsRq QBMsgsRq;
        eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(QBInvoiceAddRqModel));
        public ResponseObject WriteInvoiceAddXml(long InvoiceId, string jobId)
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
                    string errorDesc = string.Format("InvoiceAdd: cannot find trans_no or patient UPI for trans_id {0} returning empty XML", InvoiceId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }
                int custcount = db.qb_customers.Where(a => a.upi == data.upi).Count();
                if (custcount < 1)
                {
                    //Create the customer
                    //JobHelper hlp = new JobHelper();
                    //hlp.addJob(Services.JobType.CustomerAdd, data.patientId);
                    //rspObj.statusCode = 2;

                    JobHelper hlp = new JobHelper();
                    hlp.addJob(Services.JobType.CustomerAdd, data.patientId);
                    rspObj.statusCode = 2;
                    string errorDesc = string.Format("InvoiceAdd: cannot find upi {0} in qb_customer. new customer job submitted. returning empty XML", data.upi);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    return rspObj;
                    //return rspObj;

                }

                var query = (from e in db.debtor_trans
                             join invd in db.debtor_trans_details on e.trans_token equals invd.debtor_trans_no
                             join p in db.patients on e.debtor_no equals p.upi
                             join qb in db.qb_customers on p.upi equals qb.upi
                             where e.trans_no == InvoiceId
                             //where e.sync_flag == false
                             select new
                             {
                                 PONumber = e.order_,  //.ToString()
                                 RefNumber = e.reference,
                                 Memo = invd.description,
                                 TxnDate = e.tran_date,                      //.ToString("yyyy-mm-dd")         
                                 DebtorNo = qb.ListID,
                             }).ToList();
                if (query.Count < 1)
                {
                    string errorDesc = string.Format("InvoiceAdd: cannot find trans and ListID with transid  {0} in eclinic DB. returning empty XML", InvoiceId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 3;
                    return rspObj;
                }
                var tmpinvoice = query.Select(e => new TmpInvAddModel
                {
                    PONumber = e.PONumber,  //.ToString()
                    RefNumber = e.RefNumber,
                    Memo = e.Memo,
                    TxnDate = e.TxnDate.ToString("yyyy-MM-dd"),                     //.ToString("yyyy-mm-dd")  
                    DebtorNo = e.DebtorNo
                }).FirstOrDefault();
                InvoiceAddModel invoice = new InvoiceAddModel();
                invoice.PONumber = tmpinvoice.PONumber;
                invoice.RefNumber = tmpinvoice.PONumber;
                invoice.Memo = tmpinvoice.Memo;
                invoice.TxnDate = tmpinvoice.TxnDate;

                CustomerRef custref = new CustomerRef();
                log.Info("Log debtorNo =" + tmpinvoice.DebtorNo);
                custref.ListID = tmpinvoice.DebtorNo;


                invoice.CustomerNo = custref;
                var serviceItem = db.qb_settings.Where(a => a.companyid == 1).FirstOrDefault();

                var itemdetails = (from inv in db.debtor_trans
                                       join invd in db.debtor_trans_details on inv.trans_token equals invd.debtor_trans_no
                                   //join x in db.items on invd.service_id equals x.itemid
                                   where inv.trans_no == InvoiceId
                                   select new OrderedItem
                                   {
                                       ItemDescription = serviceItem.itemname,
                                       Quantity = 1,
                                       Rate = inv.ov_amount,
                                       LineItem = new LineItemRef { FullName = serviceItem.itemname, ListID= serviceItem.itemlistid },
                                       //ItemDescription = invd.description,
                                       //Quantity = invd.quantity,
                                       //Rate = invd.unit_price,
                                       //LineItem = new LineItemRef { FullName = invd.service_id },
                                   });

               
                OrderedItem[] detailsList = itemdetails.ToArray();
                invoice.InvoiceLineItems = detailsList;
                if (invoice == null || detailsList.Count() == 0)
                {
                    rspObj.statusXML = string.Empty;
                    rspObj.statusDesc = " order details not found ";
                    rspObj.statusCode = 3;
                    return rspObj;
                }
                
                BillAddress billAddy = new BillAddress();
                billAddy.Addr1 = "1, Jane Doe Close";
                billAddy.City = "Ikoyi";
                billAddy.State = "Lagos";
                billAddy.PostalCode = "23401";
                billAddy.Country = "NG";
                InvoiceAddRq InvAddreq = new InvoiceAddRq();
                InvAddreq.requestID = jobId;

                //CustItemRef custItemref = new CustItemRef();
                string debtorNo = invoice.CustomerNo.ListID;
               

                invoice.BillAddy = billAddy;
                invoice.CustomerNo = custref;
                
                InvAddreq.InvoiceAdd = invoice;

                QBXMLMsgsRq qbMsgReqs = new QBXMLMsgsRq();
                qbMsgReqs.InvoiceAddRq = InvAddreq;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer serializer =
                new XmlSerializer(typeof(QBInvoiceAddRqModel));

                QBInvoiceAddRqModel modl = new QBInvoiceAddRqModel();
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
            catch(Exception ex)
            {
                rspObj.statusDesc = ex.Message;
                rspObj.statusCode = 99;
                log.Error(ex.StackTrace, ex);
            }
            
            return rspObj;
        }

    }

    public class QBXMLMsgsRq
    {

        [XmlElementAttribute("InvoiceAddRq", IsNullable = false)]
        public InvoiceAddRq InvoiceAddRq;
        [XmlAttribute(AttributeName = "onError")]
        public string onError { get; set; }
        public QBXMLMsgsRq()
        {
            onError = "stopOnError";
        }
    }

    public class InvoiceAddRq
    {
        [XmlElementAttribute("InvoiceAdd", IsNullable = false)]
        public InvoiceAddModel InvoiceAdd;
        [XmlAttribute(AttributeName = "requestID")]
        public string requestID { get; set; }
    }

    public class InvoiceAddModel
    {

        [XmlElementAttribute("CustomerRef", Order = 1)]
        public CustomerRef CustomerNo { get; set; }
        [XmlElementAttribute("TxnDate", Order = 2)]
        public string TxnDate;
        [XmlElementAttribute("RefNumber", Order = 3)]
        public string RefNumber { get; set; }
        [XmlElementAttribute("BillAddress", Order = 4)]
        public BillAddress BillAddy;
        [XmlElementAttribute("PONumber", Order = 5)]
        public string PONumber { get; set; }
        [XmlElementAttribute("Memo", Order = 6)]
        public string Memo { get; set; }

        [XmlElementAttribute("InvoiceLineAdd", Order = 7)]
        //public List<OrderedItem> InvoiceLineItems;
        public OrderedItem[] InvoiceLineItems;


    }

    public class OrderedItem
    {
        [XmlElementAttribute("ItemRef", Order = 1)]
        public LineItemRef LineItem;
        [XmlElementAttribute("Desc", Order = 2)]
        public string ItemDescription { get; set; }
        [XmlElementAttribute("Quantity", Order = 3)]
        public int Quantity;
        [XmlElementAttribute("Rate", Order = 4)]
        public double Rate;

    }
}