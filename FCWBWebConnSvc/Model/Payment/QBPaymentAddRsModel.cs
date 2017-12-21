using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model.Payment
{
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBPaymentAddRsModel
    { 

        [XmlElementAttribute("QBXMLMsgsRs", IsNullable = false)]
        public QBXMLPaymentMsgsRs QBPaymentMsgsRs;
        public QBPaymentAddRsModel()
        {

        }
        public QBPaymentAddRsModel ReadPaymentAddRsXml(string strResponseXml)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(QBPaymentAddRsModel));
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
            QBPaymentAddRsModel responseObject;
            // Use CustomerAddRsModel Deserialize method to restore the object's state with
            //data from the XML document.
            responseObject = (QBPaymentAddRsModel)serializer.Deserialize(sr);

            //QBXMLMsgsRs qbMsgs = responseObject.QBMsgsRs;
            //CustomerAddRs rsAttributes = qbMsgs.CustAddRs;
            //CustomerAddRsViewModel custRetNode = rsAttributes.CustAddRs;

            return responseObject;
        }
        public class QBXMLPaymentMsgsRs
        {

            [XmlElementAttribute("ReceivePaymentAddRs", IsNullable = false)]
            public ReceivePaymentAddRs ReceivePaymentAddRs;
            public QBXMLPaymentMsgsRs()
            {

            }
        }
        public class ReceivePaymentAddRs
        {
            [XmlElementAttribute("ReceivePaymentRet", IsNullable = false)]
            public ReceivePaymentRet ReceivePaymentRet;
            //[XmlAttribute(AttributeName = "requestID")]
            //public string requestID { get; set; }
            [XmlAttribute(AttributeName = "statusCode")]
            public int statusCode { get; set; }
            [XmlAttribute(AttributeName = "statusSeverity")]
            public string statusSeverity { get; set; }
            [XmlAttribute(AttributeName = "statusMessage")]
            public string statusMessage { get; set; }
        }
        public class ReceivePaymentRet
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
            
            [XmlElementAttribute("TxnDate", Order = 8)]
            public string TxnDate;
            [XmlElementAttribute("RefNumber", Order = 9)]
            public string RefNumber { get; set; }
            [XmlElementAttribute("TotalAmount", Order = 10)]
            public decimal TotalAmount;
            [XmlElementAttribute("PaymentMethodRef", Order = 11)]
            public PaymentMethodRef PaymentMethodRef { get; set; }
            [XmlElementAttribute("Memo", Order = 12)]
            public string Memo { get; set; }
            [XmlElementAttribute("DepositToAccountRef", Order = 13)]
            public DepositToAccountRef DepositToAccountRef { get; set; }
            [XmlElementAttribute("UnusedPayment", Order = 14)]
            public decimal UnusedPayment { get; set; }
            [XmlElementAttribute("UnusedCredits", Order = 15)]
            public decimal UnusedCredits { get; set; }

            [XmlElementAttribute("AppliedToTxnRet", Order = 16)]
            public AppliedToTxnRet AppliedToTxnRet { get; set; }

            [XmlElementAttribute("DataExtRet", Order = 17)]
            public DataExtRet DataExtRet { get; set; }

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
        public class AppliedToTxnRet
        {
            [XmlElementAttribute("TxnID", Order = 1)]
            public string TxnID { get; set; }
            [XmlElementAttribute("TxnType", Order = 2)]
            public string TxnType { get; set; }
            [XmlElementAttribute("TxnDate", Order = 3)]
            public string TxnDate { get; set; }
            [XmlElementAttribute("RefNumber", Order = 4)]
            public string RefNumber { get; set; }
            [XmlElementAttribute("BalanceRemaining", Order = 5)]

            public decimal BalanceRemaining { get; set; }
            [XmlElementAttribute("DiscountAmount", Order = 6)]

            public decimal DiscountAmount { get; set; }
            [XmlElementAttribute("DiscountAccountRef", Order = 7)]
            public DiscountAccountRef DiscountAccountRef { get; set; }
            

        }
        public class DiscountAccountRef
        {
            [XmlElementAttribute("ListID", Order = 1)]
            public string ListID { get; set; }
            [XmlElementAttribute("FullName", Order = 2)]
            public string FullName { get; set; }
        }
        public class DataExtRet
        {
            [XmlElementAttribute("OwnerID", Order = 1)]
            public string OwnerID { get; set; }
            [XmlElementAttribute("DataExtName", Order = 2)]
            public string DataExtName { get; set; }
            [XmlElementAttribute("DataExtType", Order = 3)]
            public string DataExtType { get; set; }
            [XmlElementAttribute("DataExtValue", Order = 4)]
            public string DataExtValue { get; set; }


        }
    }

    
}