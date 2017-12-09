using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBCustomerQueryRsModel
    {
        
        [XmlElementAttribute("QBXMLMsgsRs", IsNullable = false)]
        public QBCustQrXMLMsgsRs QBMsgsRs;
        //public QBXML QBXmlObject { get; set; }
        public QBCustomerQueryRsModel()
        {

        }
        public class QBCustQrXMLMsgsRs
        {
            [XmlElementAttribute("CustomerQueryRs", IsNullable = false)]
            public CustomerQueryRs CustQueryRs;
            public QBCustQrXMLMsgsRs()
            {
                
            }
        }
        public class CustomerQueryRs
        {
            
            [XmlAttribute(AttributeName = "requestID")]
            public string requestID { get; set; }
            [XmlAttribute(AttributeName = "statusCode")]
            public int statusCode { get; set; }

            [XmlAttribute(AttributeName = "statusSeverity")]
            public string statusSeverity { get; set; }

            [XmlAttribute(AttributeName = "statusMessage")]
            public string statusMessage { get; set; }
            [XmlAttribute(AttributeName = "iteratorRemainingCount")]  //iteratorRemainingCount="3518" 
            public int iteratorRemainingCount { get; set; }
            [XmlAttribute(AttributeName = "iteratorID")]
            public string iteratorID { get; set; }
            [XmlElementAttribute("CustomerRet")]
            public CustomerQueryRet[] CustomerRet { get; set; }
            public CustomerQueryRs()
            {

            }
        }
        public class CustomerQueryRet
        {
            [XmlElement(ElementName = "ListID", Order = 1, IsNullable = false)]
            public string ListID { get; set; }
            [XmlElement(ElementName = "TimeCreated", Order = 2, IsNullable = false)]  //CCCC-MM-DDThh:mm:ss-hh:mm
            public string TimeCreated { get; set; }
            [XmlElement(ElementName = "TimeModified", Order = 3, IsNullable = false)]
            public string TimeModified { get; set; }
            [XmlElement(ElementName = "EditSequence", Order = 4, IsNullable = false)]
            public string EditSequence { get; set; }

            [XmlElement(ElementName = "Name", Order = 5, IsNullable = false)]
            public string Name { get; set; }

            [XmlElement(ElementName = "FullName", Order = 6, IsNullable = false)]
            public string FullName { get; set; }

            [XmlElement(ElementName = "IsActive", Order = 7)]
            public string IsActive { get; set; }
            [XmlElement(ElementName = "ParentRef", Order = 8)]
            public ParentRef ParentRef { get; set; }
            [XmlElement(ElementName = "Sublevel", Order = 9, IsNullable = false)]
            public string Sublevel { get; set; }
            [XmlElement(ElementName = "Salutation", Order = 10)]
            public string Salutation { get; set; }
            [XmlElement(ElementName = "FirstName", Order = 11)]
            public string FirstName { get; set; }
            [XmlElement(ElementName = "MiddleName", Order = 12)]
            public string MiddleName { get; set; }
            [XmlElement(ElementName = "LastName", Order = 13)]
            public string LastName { get; set; }
            [XmlElement(ElementName = "CompanyName", Order = 14)]
            public string CompanyName { get; set; }

            [XmlElementAttribute("BillAddress", Order = 15)]
            public BillAddress BillAddy;
            [XmlElementAttribute("SalesRepRef", Order = 16)]
            public SalesRepRef SalesRepRef;
            [XmlElement(ElementName = "Phone", Order = 17)]
            public string Phone { get; set; }
            [XmlElement(ElementName = "Email", Order = 18)]
            public string Email { get; set; }
            
            [XmlElement(ElementName = "Balance", Order = 19)]
            public string Balance { get; set; }
            [XmlElement(ElementName = "TotalBalance", Order = 20)]
            public string TotalBalance { get; set; }
            [XmlElement(ElementName = "SalesTaxCodeRef", Order = 21)]
            public SalesTaxCodeRef SalesTaxCodeRef { get; set; }
            [XmlElement(ElementName = "ItemSalesTaxRef", Order = 22)]
            public ItemSalesTaxRef ItemSalesTaxRef { get; set; }
            [XmlElement(ElementName = "JobStatus", Order = 23)]
            public string JobStatus { get; set; }

        }

        public QBCustomerQueryRsModel ReadCustomerQueryRsXml(string strResponseXml)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(QBCustomerQueryRsModel));
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
            QBCustomerQueryRsModel responseObject;
            /* Use CustomerAddRsModel Deserialize method to restore the object's state with
            data from the XML document. */
            responseObject = (QBCustomerQueryRsModel)serializer.Deserialize(sr);

            //QBXMLMsgsRs qbMsgs = responseObject.QBMsgsRs;
            //CustomerAddRs rsAttributes = qbMsgs.CustAddRs;
            //CustomerAddRsViewModel custRetNode = rsAttributes.CustAddRs;

            return responseObject;


        }
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
        private void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
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