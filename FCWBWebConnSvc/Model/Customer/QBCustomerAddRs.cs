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
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class CustomerAddRsModel
    {
        //eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomerAddRsModel));
        public string Xml { get; set; }
        [XmlElementAttribute("QBXMLMsgsRs", IsNullable = false)]
        public QBXMLMsgsRs QBMsgsRs;
        //public QBXML QBXmlObject { get; set; }
        public CustomerAddRsModel()
        {            
            
        }
        
        public CustomerAddRsModel ReadCustomerAddRsXml(string strResponseXml)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(CustomerAddRsModel));
            /* If the XML document has been altered with unknown 
              nodes or attributes, handle them with the 
              UnknownNode and UnknownAttribute events.*/
            serializer.UnknownNode += new
            XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new
            XmlAttributeEventHandler(serializer_UnknownAttribute);

            // A FileStream is needed to read the XML document.
            StringReader sr = new StringReader(strResponseXml);
            // Declare an object variable of the type to be deserialized.
            CustomerAddRsModel responseObject;
            /* Use CustomerAddRsModel Deserialize method to restore the object's state with
            data from the XML document. */
            responseObject = (CustomerAddRsModel)serializer.Deserialize(sr);

            //QBXMLMsgsRs qbMsgs = responseObject.QBMsgsRs;
            //CustomerAddRs rsAttributes = qbMsgs.CustAddRs;
            //CustomerAddRsViewModel custRetNode = rsAttributes.CustAddRs;

            return responseObject;
            

        }
        public class QBXMLMsgsRs
        {
            [XmlElementAttribute("CustomerAddRs", IsNullable = false)]
            public CustomerAddRs CustAddRs;
            public QBXMLMsgsRs()
            {
            }
        }
        public class CustomerAddRs
        {
            [XmlElementAttribute("CustomerRet", IsNullable = false)]
            public CustomerAddRsViewModel CustAddRs;//statusCode="0" statusSeverity="Info" statusMessage="Status OK"
            [XmlAttribute(AttributeName = "statusCode")]  
            public int statusCode { get; set; }
            [XmlAttribute(AttributeName = "statusSeverity")]
            public string statusSeverity { get; set; }
            [XmlAttribute(AttributeName = "statusMessage")]
            public string statusMessage { get; set; }
        }
        //[XmlElementAttribute("CustomerAdd", IsNullable = false)]
        public class CustomerAddRsViewModel
        { 
            [XmlElement(ElementName = "ListID", Order = 1, IsNullable = false)]
            public string ListID { get; set; }
            [XmlElement(ElementName = "TimeCreated", Order = 2, IsNullable = false )]  //CCCC-MM-DDThh:mm:ss-hh:mm
            public string TimeCreated { get; set; }
            [XmlElement(ElementName = "TimeModified", Order = 3, IsNullable = false)]
            public string TimeModified { get; set; }
            [XmlElement(ElementName = "EditSequence", Order = 4, IsNullable = false)]
            public string EditSequence { get; set; }

            [XmlElement(ElementName = "Name", Order = 5, IsNullable = false)]
            public string Name { get; set; }
           
            [XmlElement(ElementName = "FullName", Order = 6, IsNullable = false)]
            public string FullName { get; set; }
            
            [XmlElement(ElementName = "Isactive", Order = 7)]
            public bool IsActive { get; set; }
            [XmlElement(ElementName = "Sublevel", Order = 8, IsNullable = false)]
            public string Sublevel { get; set; }
            [XmlElement(ElementName = "CompanyName", Order = 9)]
            public string CompanyName { get; set; }

            [XmlElement(ElementName = "Salutation", Order = 10)]
            public string Salutation { get; set; }
            [XmlElement(ElementName = "FirstName", IsNullable = false, Order = 11)]
            public string FirstName { get; set; }
            [XmlElement(ElementName = "Middlename", Order = 12)]
            public string MiddleName { get; set; }
            [XmlElement(ElementName = "Lastname", Order = 13)]
            public string LastName { get; set; }
            [XmlElementAttribute("BillAddress", Order = 14)]
            public BillAddress BillAddy;
            [XmlElement(ElementName = "Phone", Order = 15)]
            public string Phone { get; set; }
            [XmlElement(ElementName = "Email", Order = 16)]
            public string Email { get; set; }
            [XmlElement(ElementName = "Contact", Order = 17)]
            public string Contact { get; set; }
        }
        public class BillAddress
        {
            [XmlElement(ElementName = "Addr1", Order = 1)]
            public string Addr1 { get; set; }
            [XmlElement(ElementName = "City", Order = 2)]
            public string City { get; set; }
            [XmlElement(ElementName = "State", Order = 3)]
            public string State { get; set; }
            [XmlElement(ElementName = "PostalCode", Order = 4)]
            public string PostalCode { get; set; }
            [XmlElement(ElementName = "Country", Order = 5)]
            public string Country { get; set; }
        }
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
        private void serializer_UnknownNode   (object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        private void serializer_UnknownAttribute
        (object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute " +
            attr.Name + "='" + attr.Value + "'");
        }
    }
}