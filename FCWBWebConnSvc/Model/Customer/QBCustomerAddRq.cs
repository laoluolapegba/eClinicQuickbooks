using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using FCQB.Data;
using System.IO;
using System.Text;
using log4net;

namespace FCQBWebConnAPI.Model
{
    //[XmlRootAttribute("qbxml", IsNullable = false)]
    //public class CustomerAddXMLHeader
    //{

    //}
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class CustomerAddModel
    {
        eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomerAddModel));
        public string Xml { get; set; }
        [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        public QBXMLMsgsRq QBMsgsRq;
        //public QBXML QBXmlObject { get; set; }
        public CustomerAddModel()
        {            
            
        }
        public ResponseObject WriteCustomerAddXml(string CustId, string jobId)
        {
            string strQXML = string.Empty;
            ResponseObject rspObj = new ResponseObject();
            try
            {
                db = new eClatModel();
                var customer = (from e in db.patients
                                where e.patient_id == CustId
                                //where e.sync_flag == false
                                select new CustomerAddViewModel
                                {
                                    Name = e.surname, 
                                    FirstName = e.forename,
                                    MiddleName = e.middle_name,
                                    LastName = e.surname,
                                    Salutation = e.title,
                                    IsActive = !e.inactive,
                                    Phone = e.phone,
                                    Email = e.email,
                                    //1Contact = e.nok_full_name,
                                    AccountNumber = e.upi,
                                    CardNo = e.card_number

                                }).FirstOrDefault();
                //e.forename + " " +  e.surname + " - " + e.card_number, 
                if (customer == null)
                {
                    string errorDesc = string.Format("CustomerAdd: cannot find patient with patientid  {0} in eclinic DB. returning empty XML", CustId);
                    log.Info(errorDesc);
                    rspObj.statusDesc = errorDesc;
                    rspObj.statusCode = 1;
                    return rspObj;
                }
                string tempname = (customer.LastName + " " +  customer.FirstName + " " + customer.MiddleName +"-"+ customer.CardNo); //.Substring(0, 28);

                customer.Name = (tempname.Length > 28 ? tempname.Substring(0, 27) : tempname); // + customer.CardNo;
                
                //Trim off Middlename to Initial only
                if (customer.MiddleName != null)
                {
                    if (customer.MiddleName != string.Empty)
                    {
                        customer.MiddleName = customer.MiddleName.Substring(0, 1);
                    }
                }
                string tempcontact = customer.LastName + " " + customer.FirstName + " " + customer.MiddleName;
                //customer.Contact = (tempcontact.Length > 40 ? tempcontact.Substring(0, 40) : tempcontact);
                //Trim off phone numbers
                if (customer.Phone != null)
                {
                    if (customer.Phone.Length > 11)
                    {
                        customer.Phone = customer.Phone.Substring(0, 11);
                    }
                }
                else
                {
                    customer.Phone = "";
                }
                string patientAddress = db.patients.Where(a => a.patient_id == CustId).Select(a => a.address).FirstOrDefault();

                BillAddress billAddy = new BillAddress();
                if (!string.IsNullOrEmpty(patientAddress))
                {
                    if (patientAddress.Length > 0 && patientAddress.Length < 41)
                    {
                        billAddy.Addr1 = patientAddress.Substring(0, patientAddress.Length - 1); ///its a zero based index
                    }
                    else
                    { //greater than 41
                        billAddy.Addr1 = patientAddress.Substring(0, 40);
                    }
                }
                else //empty string
                { billAddy.Addr1 = "-"; }
                //if (patientAddress.Length > 39)
                //{
                //    billAddy.Addr1 = patientAddress.Substring(0, 40);
                //    if (patientAddress.Length > 40)
                //    {

                //        if (patientAddress.Length > 79)
                //        {
                //            billAddy.Addr2 = patientAddress.Substring(40, 80);
                //        }
                //        else
                //        {
                //            billAddy.Addr2 = patientAddress.Substring(40, patientAddress.Length);
                //        }
                //    }
                //    if (patientAddress.Length > 80)
                //    {
                //        billAddy.Addr3 = patientAddress.Substring(80, patientAddress.Length);
                //    }
                //}
                //else
                //    billAddy.Addr1 = patientAddress;

                billAddy.City = "--";
                billAddy.State = "Lagos";
                billAddy.PostalCode = "23401";
                CustomerAddRq Addreq = new CustomerAddRq();
                Addreq.requestID = jobId;
                customer.BillAddy = billAddy;
                Addreq.CustAdd = customer;
                //Addreq.CustAdd.

                QBXMLMsgsRq qbMsgReqs = new QBXMLMsgsRq();
                qbMsgReqs.CustAddRq = Addreq;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                XmlSerializer serializer =
                new XmlSerializer(typeof(CustomerAddModel));

                CustomerAddModel modl = new CustomerAddModel();
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
        //[XmlRootAttribute("QBXML", IsNullable = false)]
        //public class QBXML
        //{
            
        //    [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        //    public QBXMLMsgsRq QBMsgsRq;
            
        //}
        public class QBXMLMsgsRq
        {
            [XmlElementAttribute("CustomerAddRq", IsNullable = false)]
            public CustomerAddRq CustAddRq;
            [XmlAttribute(AttributeName = "onError")]
            public string onError { get; set; }
            public QBXMLMsgsRq()
            {
                onError = "stopOnError";
            }
        }
        public class CustomerAddRq
        {
            [XmlElementAttribute("CustomerAdd", IsNullable = false)]
            public CustomerAddViewModel CustAdd;
            [XmlAttribute(AttributeName = "requestID")]
            public string requestID { get; set; }
        }
        //[XmlElementAttribute("CustomerAdd", IsNullable = false)]
        public class CustomerAddViewModel
        {            
            [XmlElement(ElementName = "Name", Order = 1)]
            public string Name { get; set; }
            [XmlElement(ElementName = "IsActive", Order = 2)]
            public bool IsActive { get; set; }
            [XmlElement(ElementName = "Salutation", Order = 3)]
            public string Salutation { get; set; }
            [XmlElement(ElementName = "FirstName", IsNullable = false, Order = 4)]
            public string FirstName { get; set; }
            [XmlElement(ElementName = "MiddleName", Order = 5)]
            public string MiddleName { get; set; }
            [XmlElement(ElementName = "LastName", Order = 6)]
            public string LastName { get; set; }
            [XmlElementAttribute("BillAddress", Order = 7)]
            public BillAddress BillAddy;
            [XmlElement(ElementName = "Phone", Order = 8)]
            public string Phone { get; set; }
            [XmlElement(ElementName = "Email", Order = 9)]
            public string Email { get; set; }
            //[XmlElement(ElementName = "Contact", Order = 10)]
            //public string Contact { get; set; }
            //[System.Xml.Serialization.XmlIgnore]
            [XmlElement(ElementName = "AccountNumber", Order = 10)]
            public string AccountNumber { get; set; }
            [System.Xml.Serialization.XmlIgnore]
            public string CardNo { get; set; }
        }
        
       
    }

    
}