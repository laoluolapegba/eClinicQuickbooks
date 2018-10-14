using FCQB.Data;
using FCQBWebConnAPI.Model.Vendor;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model.Bill
{

    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBBillAddRqModel
    {
        [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        public QBXMLBillMsgsRq QBMsgsRq;
        eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(QBBillAddRqModel));
        public ResponseObject WriteBillAddXml(long InvoiceId, string jobId)
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
                    string errorDesc = string.Format("BillAdd: cannot find trans_no or patient UPI for trans_id {0} returning empty XML", InvoiceId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }
                int custcount = db.qb_customers.Where(a => a.upi == data.upi).Count();
                if (custcount < 1)
                {
                    //Create the customer

                    JobHelper hlp = new JobHelper();
                    hlp.addJob(Services.JobType.CustomerAdd, data.patientId);
                    rspObj.statusCode = 2;
                    string errorDesc = string.Format("BillAdd: cannot find upi {0} in qb_customer. new customer job submitted. returning empty XML", data.upi);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    return rspObj;
                    //return rspObj;

                }
                //var query = (from e in db.debtor_trans
                //            join inv in db.patients on e.debtor_no equals inv.patient_id
                //            where e.trans_no == InvoiceId
                //            //where e.sync_flag == false
                //            select new
                //            {
                //                PONumber = e.order_,  //.ToString()
                //                RefNumber = e.reference,
                //                Memo = e.clinic_token,
                //                TxnDate = e.tran_date,                      //.ToString("yyyy-mm-dd")         
                //                DebtorNo = inv.ext_sys_id,
                //            }).ToList();
                var query = (from e in db.debtor_trans
                             join invd in db.debtor_trans_details on e.trans_token equals invd.debtor_trans_no
                             join p in db.patients on e.debtor_no equals p.upi
                             join qb in db.qb_customers on p.upi equals qb.upi
                             where e.trans_no == InvoiceId
                             //where e.sync_flag == false
                             select new
                             {
                                 PONumber = e.trans_no,  //.ToString()
                                 RefNumber = e.reference,
                                 Memo = invd.description,
                                 TxnDate = e.tran_date,                      //.ToString("yyyy-mm-dd")         
                                 DebtorNo = qb.ListID,
                             }).ToList();
                if (query.Count < 1)
                {
                    string errorDesc = string.Format("BillAdd: cannot find trans and ListID with transid  {0} in eclinic DB. returning empty XML", InvoiceId);
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
                BillAddModel bill = new BillAddModel();
                bill.RefNumber = tmpinvoice.RefNumber;
                bill.Memo = tmpinvoice.Memo;
                bill.TxnDate = tmpinvoice.TxnDate;

                VendorRef vendorref = new VendorRef();
                log.Info("Log vendorNo =" + tmpinvoice.DebtorNo);
                vendorref.ListID = tmpinvoice.DebtorNo;


                bill.VendorNo = vendorref;
                var serviceItem = db.qb_settings.Where(a => a.companyid == 1).FirstOrDefault();

                //var itemdetails = (from invd in db.debtor_trans_details
                //                   join inv in db.debtor_trans on invd.debtor_trans_no equals inv.trans_token
                //                   //join x in db.items on invd.service_id equals x.itemid
                //                   where inv.trans_no == InvoiceId
                //                   select new BillItems
                //                   {
                //                       //Desc = invd.description,
                //                       //Quantity = invd.quantity,
                //                       //Cost = invd.unit_price,
                //                       //LineItemRef = new LineItemRef { FullName = x.itemid },
                //                       Desc = invd.description,
                //                       Quantity = 1,
                //                       Cost = inv.ov_amount,
                //                       LineItemRef = new LineItemRef { FullName = invd.service_id },

                //                   });
                var itemdetails = (from inv in db.debtor_trans
                                   join invd in db.debtor_trans_details on inv.trans_token equals invd.debtor_trans_no
                                   //join x in db.items on invd.service_id equals x.itemid
                                   where inv.trans_no == InvoiceId
                                   select new BillItems
                                   {
                                       Desc = serviceItem.itemname,
                                       Quantity = 1,
                                       Cost = inv.ov_amount,
                                       LineItemRef = new LineItemRef { FullName = serviceItem.itemname, ListID = serviceItem.itemlistid },
                                       //ItemDescription = invd.description,
                                       //Quantity = invd.quantity,
                                       //Rate = invd.unit_price,
                                       //LineItem = new LineItemRef { FullName = invd.service_id },
                                   });

                BillItems[] detailsList = itemdetails.ToArray();
                bill.ItemLineAdd = detailsList;

                if (bill == null || detailsList.Count() == 0)
                {
                    rspObj.statusXML = string.Empty;
                    rspObj.statusDesc = " order details not found ";
                    rspObj.statusCode = 3;
                    return rspObj;
                }
                

                //StringBuilder strQXML = new StringBuilder();
                
                //VendorAddress billAddy = new VendorAddress();
                //billAddy.Addr1 = "1, Jane Doe Close";
                //billAddy.City = "Ikoyi";
                //billAddy.State = "Lagos";
                //billAddy.PostalCode = "23401";
                //billAddy.Country = "NG";
                BillAddRq BillAddreq = new BillAddRq();
                //InvAddreq.requestID = jobId;

                //CustItemRef custItemref = new CustItemRef();
                string vendorNo = bill.VendorNo.ListID;

                //bill.VendorNo = billAddy;
                BillAddreq.BillAdd = bill;

                QBXMLBillMsgsRq qbMsgReqs = new QBXMLBillMsgsRq();
                qbMsgReqs.BillAddRq = BillAddreq;


                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer serializer =
                new XmlSerializer(typeof(QBBillAddRqModel));

                QBBillAddRqModel modl = new QBBillAddRqModel();
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

    public class QBXMLBillMsgsRq
    {

        [XmlElementAttribute("BillAddRq", IsNullable = false)]
        public BillAddRq BillAddRq;
        [XmlAttribute(AttributeName = "onError")]
        public string onError { get; set; }
        public QBXMLBillMsgsRq()
        {
            onError = "stopOnError";
        }
    }

    public class BillAddRq
    {
        [XmlElementAttribute("BillAdd", IsNullable = false)]
        public BillAddModel BillAdd;
        //[XmlAttribute(AttributeName = "requestID")]
        //public string requestID { get; set; }
    }

    public class BillAddModel
    {

        [XmlElementAttribute("VendorRef", Order = 1)]
        public VendorRef VendorNo { get; set; }
        [XmlElementAttribute("TxnDate", Order = 2)]
        public string TxnDate;
        [XmlElementAttribute("DueDate", Order = 3)]
        public string DueDate;
        [XmlElementAttribute("RefNumber", Order = 4)]
        public string RefNumber { get; set; }

        [XmlElementAttribute("Memo", Order = 5)]
        public string Memo { get; set; }

        //[XmlElementAttribute("AppliedToTxnAdd", Order = 6)]
        //public AppliedToTxnAdd AppliedToTxnAdd { get; set; }
        [XmlElementAttribute("ItemLineAdd", Order = 6)]
        public BillItems[] ItemLineAdd { get; set; }

    }


    public class BillItems
    {
        [XmlElementAttribute("ItemRef", Order = 1)]
        public LineItemRef LineItemRef { get; set; }
        
        [XmlElementAttribute("Desc", Order = 2)]
        public string Desc { get; set; }
        [XmlElementAttribute("Quantity", Order = 3)]
        public int Quantity { get; set; }

        [XmlElementAttribute("Cost", Order = 4)]
        public double Cost { get; set; }
        [XmlElementAttribute("Amount", Order = 5)]
        public double BillAmount { get; set; }

    }

   
}