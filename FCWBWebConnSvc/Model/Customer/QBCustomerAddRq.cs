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
        public string WriteCustomerAddXml(string CustId, string jobId)
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
                               Contact = e.nok_full_name                               
                           }).FirstOrDefault();
            if(customer == null)
            {
                log.Info(string.Format("CustomerAdd: cannot find patient with transid  {0} in eclinic DB. returning empty XML", CustId));
                return string.Empty;
            }
            //Trim off Middlename to Initial only
            if (customer.MiddleName != string.Empty)
            {
                customer.MiddleName = customer.MiddleName.Substring(0, 1);
            }
            //Trim off phone numbers
            if (customer.Phone.Length > 11)
            {
                customer.Phone = customer.Phone.Substring(0, 11);
            }
            //StringBuilder strQXML = new StringBuilder();
            string strQXML = string.Empty;
            BillAddress billAddy = new BillAddress();
            billAddy.Addr1 = "1, Jane Doe Close";
            billAddy.City = "Ikoyi";
            billAddy.State = "Lagos";
            billAddy.PostalCode = "23401";
            CustomerAddRq Addreq = new CustomerAddRq();
            Addreq.requestID = jobId;
            customer.BillAddy = billAddy;
            Addreq.CustAdd = customer;

            QBXMLMsgsRq qbMsgReqs = new QBXMLMsgsRq();
            qbMsgReqs.CustAddRq = Addreq;

            //QBXML QBXmlObject = new QBXML();
            //QBXmlObject.QBMsgsRq = qbMsgReqs;
            // Create an instance of the XmlSerializer class;
            // specify the type of object to serialize.
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
            
           
            
            //serializer.Serialize(writer, modl, ns);
            //writer.Close();

            return strQXML;
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
            [XmlElement(ElementName = "Contact", Order = 10)]
            public string Contact { get; set; }
        }
        
       
    }

    
}