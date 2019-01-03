using FCQB.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model.ItemInventory
{
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBItemAddRqModel
    {
        [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        public QBXMLMsgsRq QBMsgsRq;
        eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(QBItemAddRqModel));

        public QBItemAddRqModel()
        {

        }
        public ResponseObject WriteItemInventoryAddXml(string ItemId, string jobId)
        {
            string strQXML = string.Empty;
            ResponseObject rspObj = new ResponseObject();
            try
            {
                db = new eClatModel();
                int itemid = int.Parse(ItemId);
                var newItem = (from e in db.debtor_trans_details
                                where e.id == itemid
                               //where e.sync_flag == false
                               select new ItemInventoryAddModel
                                {
                                    Name = e.description,
                                    //ReorderPoint = e.reorder_level,                                    
                                    IsActive = !e.inactive,
                                    QuantityOnHand = 200,
                                    SalesDesc = e.description,
                                    SalesPrice = e.unit_price,        
                                    cogsAcctCode = e.cogs_account,
                                    incomeAcctCode = e.sales_account,
                                    assetAcctCode = e.inventory_account                    
                                   
                                }).FirstOrDefault();
                if (newItem == null)
                {
                    string errorDesc = string.Format("ItemAdd: cannot find item with ItemId  {0} in eclinic DB debtor_trans_details. returning empty XML", ItemId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }
                if (newItem.Name.Length > 31)
                {
                    newItem.Name = newItem.Name.Substring(0, 30);
                }
                //get the Accts
                var incomeAcct = (from e in db.qb_coas
                               where e.ItemId == newItem.incomeAcctCode
                                     select new IncomeAccountRef
                                     {
                                         ListID = e.ListID,
                                         FullName = e.Name
                                     }).FirstOrDefault();
                if (incomeAcct == null)
                {
                    string errorDesc = string.Format("ItemAdd: no income acct defined. returning empty XML", ItemId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }
                var cogsacct = (from e in db.qb_coas
                                     where e.ItemId == newItem.cogsAcctCode
                                select new COGSAccountRef
                                     {
                                         ListID = e.ListID,
                                         FullName = e.Name
                                     }).FirstOrDefault();
                if (cogsacct == null)
                {
                    string errorDesc = string.Format("ItemAdd: no COGS acct defined. returning empty XML", ItemId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }

                var assetacct = (from e in db.qb_coas
                                 where e.ItemId == newItem.assetAcctCode
                                 select new AssetAccountRef
                                 {
                                     ListID = e.ListID,
                                     FullName = e.Name
                                 }).FirstOrDefault();
                if (assetacct == null)
                {
                    string errorDesc = string.Format("ItemAdd: no Asset acct defined. returning empty XML", ItemId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }
                newItem.COGSAccountRef = cogsacct;
                newItem.AssetAccountRef = assetacct;
                newItem.IncomeAccountRef = incomeAcct;

                
                
                ItemAddRq Addreq = new ItemAddRq();
                Addreq.requestID = jobId;                
                Addreq.ItemInventoryAdd = newItem;
                //Addreq.CustAdd.

                QBXMLMsgsRq qbMsgReqs = new QBXMLMsgsRq();
                qbMsgReqs.ItemAddRq = Addreq;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer serializer =
                new XmlSerializer(typeof(QBItemAddRqModel));

                QBItemAddRqModel modl = new QBItemAddRqModel();
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

        [XmlElementAttribute("ItemInventoryAddRq", IsNullable = false)]
        public ItemAddRq ItemAddRq;
        [XmlAttribute(AttributeName = "onError")]
        public string onError { get; set; }
        public QBXMLMsgsRq()
        {
            onError = "stopOnError";
        }
    }

    public class ItemAddRq
    {
        [XmlElementAttribute("ItemInventoryAdd", IsNullable = false)]
        public ItemInventoryAddModel ItemInventoryAdd;
        [XmlAttribute(AttributeName = "requestID")]
        public string requestID { get; set; }
    }

    public class ItemInventoryAddModel
    {

        [XmlElementAttribute("Name", Order = 1)]
        public string Name { get; set; }

        [XmlElementAttribute("IsActive", Order = 2)]
        public bool IsActive { get; set; }


        [XmlElementAttribute("SalesDesc", Order = 3)]
        public string SalesDesc { get; set; }

        [XmlElementAttribute("SalesPrice", Order = 4)]
        public double SalesPrice { get; set; }


        //[System.Xml.Serialization.XmlIgnore]
        [XmlElementAttribute("IncomeAccountRef", Order = 5)]
        public IncomeAccountRef IncomeAccountRef { get; set; }

        //[System.Xml.Serialization.XmlIgnore]
        [XmlElementAttribute("COGSAccountRef", Order = 6)]
        public COGSAccountRef COGSAccountRef { get; set; }
        
        [XmlElementAttribute("ReorderPoint", Order = 7)]
        [System.Xml.Serialization.XmlIgnore]
        public int ReorderPoint { get; set; }
        [XmlElementAttribute("QuantityOnHand", Order = 8)]
        [System.Xml.Serialization.XmlIgnore]
        public int QuantityOnHand { get; set; }
        [XmlElementAttribute("TotalValue", Order = 9)]
        [System.Xml.Serialization.XmlIgnore]
        public decimal TotalValue { get; set; }
        [XmlElementAttribute("InventoryDate", Order = 10)]
        [System.Xml.Serialization.XmlIgnore]
        public DateTime InventoryDate { get; set; }



        //[System.Xml.Serialization.XmlIgnore]
        [XmlElementAttribute("AssetAccountRef", Order = 11)]
        public AssetAccountRef AssetAccountRef { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public string cogsAcctCode { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public string assetAcctCode { get; set; }
        [System.Xml.Serialization.XmlIgnore]
        public string incomeAcctCode { get; set; }

        //public DateTime InventoryDate { get; set; }

    }
    public class COGSAccountRef
    {
        [XmlElementAttribute("ListID", Order = 1)]
        public string ListID { get; set; }
        [XmlElementAttribute("FullName", Order = 2)]
        public string FullName { get; set; }
    }
    public class AssetAccountRef
    {
        [XmlElementAttribute("ListID", Order = 1)]
        public string ListID { get; set; }
        [XmlElementAttribute("FullName", Order = 2)]
        public string FullName { get; set; }
    }
    public class IncomeAccountRef
    {
        [XmlElementAttribute("ListID", Order = 1)]
        public string ListID { get; set; }
        [XmlElementAttribute("FullName", Order = 2)]
        public string FullName { get; set; }
    }
}