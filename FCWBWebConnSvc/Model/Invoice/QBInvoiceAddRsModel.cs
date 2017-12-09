using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    //<QBXML>
//<QBXMLMsgsRs>
//<InvoiceAddRs requestID = "10" statusCode="3000" statusSeverity="Error"
//statusMessage="The given object ID &quot;1&quot; in the field &quot;list id&quot; is invalid. " />
//</QBXMLMsgsRs>
//</QBXML>

   
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBInvoiceAddRsModel
    {
        [XmlElementAttribute("QBXMLMsgsRs", IsNullable = false)]
        public QBXMLInvMsgsRs QBInvMsgsRs;
        public QBInvoiceAddRsModel()
        {

        }
        public QBInvoiceAddRsModel ReadInvoiceAddRsXml(string strResponseXml)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(QBInvoiceAddRsModel));
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
            QBInvoiceAddRsModel responseObject;
            // Use CustomerAddRsModel Deserialize method to restore the object's state with
            //data from the XML document.
            responseObject = (QBInvoiceAddRsModel)serializer.Deserialize(sr);

            //QBXMLMsgsRs qbMsgs = responseObject.QBMsgsRs;
            //CustomerAddRs rsAttributes = qbMsgs.CustAddRs;
            //CustomerAddRsViewModel custRetNode = rsAttributes.CustAddRs;

            return responseObject;
        }
        public class QBXMLInvMsgsRs
        {

            [XmlElementAttribute("InvoiceAddRs", IsNullable = false)]
            public InvoiceAddRs InvoiceAddRs;
            public  QBXMLInvMsgsRs()
            {

            }
        }

        public class InvoiceAddRs
        {
            [XmlElementAttribute("InvoiceRet", IsNullable = false)]
            public InvoiceRetModel InvoiceAdd;
            [XmlAttribute(AttributeName = "requestID")]
            public string requestID { get; set; }
            [XmlAttribute(AttributeName = "statusCode")]
            public int statusCode { get; set; }
            [XmlAttribute(AttributeName = "statusSeverity")]
            public string statusSeverity { get; set; }
            [XmlAttribute(AttributeName = "statusMessage")]
            public string statusMessage { get; set; }
        }

        public class InvoiceRetModel
        {

            [XmlElement(ElementName = "TxnID", Order = 1, IsNullable = false)]
            public string TxnID { get; set; }
            [XmlElement(ElementName = "TimeCreated", Order = 2, IsNullable = false)]  //CCCC-MM-DDThh:mm:ss-hh:mm
            public string TimeCreated { get; set; }
            [XmlElement(ElementName = "TimeModified", Order = 3, IsNullable = false)]
            public string TimeModified { get; set; }
            [XmlElement(ElementName = "EditSequence", Order = 4, IsNullable = false)]
            public string EditSequence { get; set; }
            [XmlElement(ElementName = "TxnNumber", Order = 5, IsNullable = false)]
            public string TxnNumber { get; set; }
            [XmlElementAttribute("CustomerRef", Order = 6)]
            public CustomerRef CustomerNo { get; set; }
            [XmlElementAttribute("ARAccountRef", Order = 7)]
            public ARAccountRef AcctNo { get; set; }
            [XmlElementAttribute("TemplateRef", Order = 8)]
            public TemplateRef TemplRef { get; set; }
            [XmlElementAttribute("TxnDate", Order = 9)]
            public string TxnDate;
            [XmlElementAttribute("RefNumber", Order = 10)]
            public string RefNumber { get; set; }
            [XmlElementAttribute("BillAddress", Order = 11)]
            public BillAddress BillAddy;

            [XmlElementAttribute("ShipAddress", Order = 12)]
            public ShipAddress ShipAddy;

            [XmlElementAttribute("IsPending", Order = 13)]
            public string IsPending { get; set; }
            [XmlElementAttribute("IsFinanceCharge", Order = 14)]
            public string IsFinanceCharge { get; set; }
            [XmlElementAttribute("TermsRef", Order = 15)]
            public TermsRef TermsofRef { get; set; }

            [XmlElementAttribute("DueDate", Order = 16)]
            public string DueDate;
            [XmlElementAttribute("SalesRepRef", Order = 17)]
            public SalesRepRef SalesRep { get; set; }
            [XmlElementAttribute("ShipDate", Order = 18)]
            public string ShipDate;
            [XmlElementAttribute("Subtotal", Order = 19)]
            public decimal Subtotal { get; set; }
            [XmlElementAttribute("ItemSalesTaxRef", Order = 20)]
            public ItemSalesTaxRef SalesTaxRef { get; set; }
            [XmlElementAttribute("SalesTaxPercentage", Order = 21)]
            public decimal SalesTaxPercentage { get; set; }
            [XmlElementAttribute("SalesTaxTotal", Order = 22)]
            public decimal SalesTaxTotal { get; set; }

            [XmlElementAttribute("AppliedAmount", Order = 23)]
            public decimal AppliedAmount { get; set; }
            [XmlElementAttribute("BalanceRemaining", Order = 24)]
            public decimal BalanceRemaining { get; set; }


            [XmlElementAttribute("Memo", Order = 25)]
            public string Memo { get; set; }

            [XmlElementAttribute("IsPaid", Order = 26)]
            public string IsPaid { get; set; }
            [XmlElementAttribute("IsToBePrinted", Order = 27)]
            public string IsToBePrinted { get; set; }
            [XmlElementAttribute("CustomerSalesTaxCodeRef", Order = 28)]
            public CustomerSalesTaxCodeRef CustTaxCodeRef { get; set; }
            /* The XmlArrayAttribute changes the XML element name
             from the default of "OrderedItems" to "Items". */
            //[XmlArrayAttribute("InvoiceLineRet", Order = 29)]
            [XmlElementAttribute("InvoiceLineRet", Order =29)]
            public InvoiceLineRet[] InvoiceLineItems;

            public class Utf8StringWriter : StringWriter
            {
                public override Encoding Encoding => Encoding.UTF8;
            }

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