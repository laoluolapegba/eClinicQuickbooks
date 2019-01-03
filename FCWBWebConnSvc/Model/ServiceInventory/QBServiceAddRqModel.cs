using FCQB.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model.ServiceInventory
{
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBServiceAddRqModel
    {
        [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        public QBXMLMsgsRq QBMsgsRq;
        eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(QBServiceAddRqModel));

        public QBServiceAddRqModel()
        {

        }
        public ResponseObject WriteServiceInventoryAddXml(string ServiceId, string jobId)
        {
            string strQXML = string.Empty;
            ResponseObject rspObj = new ResponseObject();
            try
            {
                db = new eClatModel();
                int svcId = int.Parse(ServiceId);
                var newService = (from e in db.debtor_trans_details
                                where e.id == svcId
                                  //where e.sync_flag == false
                                  select new 
                                {
                                    Name = e.description,
                                    Price = e.unit_price,
                                    Description = e.description , //+ " " + e.detail_token,
                                    incomeAcct = e.sales_account

                                }).FirstOrDefault();
                if (newService == null)
                {
                    string errorDesc = string.Format("ItemAdd: cannot find item with ServiceId  {0} in eclinic DB services. returning empty XML", ServiceId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }

                var acctRef = (from e in db.qb_coas
                                  where e.ItemId == newService.incomeAcct
                               select new AccountRef
                                  {
                                      ListID = e.ListID,
                                      FullName = e.Name
                                  }).FirstOrDefault();

                if (acctRef == null)
                {
                    string errorDesc = string.Format("ServiceAdd: no income acct defined. returning empty XML", ServiceId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }

                //var acctRef = new AccountRef
                //{
                //    FullName = "Consulting Income",
                //    ListID = "80000006-1510773375"
                //};
                

                SalesOrPurchase sp = new SalesOrPurchase();
                sp.Price = newService.Price;
                sp.Desc = newService.Description;
                sp.AccountRef = acctRef;


                ServiceInventoryAddModel svImodel = new ServiceInventoryAddModel();
                svImodel.SalesOrPurchase = sp;
                svImodel.Name = sp.Desc;

                if (svImodel.Name.Length > 31)
                {
                    svImodel.Name = svImodel.Name.Substring(0, 30);
                }


                ServiceAddRq Addreq = new ServiceAddRq();
                Addreq.requestID = jobId;          
                      
                Addreq.ServiceInventoryAdd = svImodel;
                //Addreq.CustAdd.

                QBXMLMsgsRq qbMsgReqs = new QBXMLMsgsRq();
                qbMsgReqs.ServiceAddRq = Addreq;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer serializer =
                new XmlSerializer(typeof(QBServiceAddRqModel));

                QBServiceAddRqModel modl = new QBServiceAddRqModel();
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
    public class QBXMLMsgsRq
    {

        [XmlElementAttribute("ItemServiceAddRq", IsNullable = false)]
        public ServiceAddRq ServiceAddRq;
        [XmlAttribute(AttributeName = "onError")]
        public string onError { get; set; }
        public QBXMLMsgsRq()
        {
            onError = "stopOnError";
        }
    }

    public class ServiceAddRq
    {
        [XmlElementAttribute("ItemServiceAdd", IsNullable = false)]
        public ServiceInventoryAddModel ServiceInventoryAdd;
        [XmlAttribute(AttributeName = "requestID")]
        public string requestID { get; set; }
    }

    public class ServiceInventoryAddModel
    {

        [XmlElementAttribute("Name", Order = 1)]
        public string Name { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        [XmlElementAttribute("IsActive", Order = 2)]
        public bool IsActive { get; set; }


        //[System.Xml.Serialization.XmlIgnore]
        [XmlElementAttribute("SalesOrPurchase", Order = 3)]
        public SalesOrPurchase SalesOrPurchase { get; set; }

        //[XmlElementAttribute("ReorderPoint", Order = 4)]
        //[System.Xml.Serialization.XmlIgnore]
        //public int ReorderPoint { get; set; }
        //[XmlElementAttribute("QuantityOnHand", Order = 5)]
        //[System.Xml.Serialization.XmlIgnore]
        //public int QuantityOnHand { get; set; }
        //[XmlElementAttribute("TotalValue", Order = 6)]
        //[System.Xml.Serialization.XmlIgnore]
        //public decimal TotalValue { get; set; }
        //[XmlElementAttribute("InventoryDate", Order = 7)]
        //[System.Xml.Serialization.XmlIgnore]
        //public DateTime InventoryDate { get; set; }

    }
    public class SalesOrPurchase
    {
        [XmlElementAttribute("Desc", Order = 1)]
        public string Desc { get; set; }
        [XmlElementAttribute("Price", Order = 2)]
        public double Price { get; set; }

        [XmlElementAttribute("AccountRef", Order = 3)]
        public AccountRef AccountRef { get; set; }
    }
    public class AccountRef
    {
        [XmlElementAttribute("ListID", Order = 1)]
        public string ListID { get; set; }

        [XmlElementAttribute("FullName", Order = 2)]
        public string FullName { get; set; }
    }
}