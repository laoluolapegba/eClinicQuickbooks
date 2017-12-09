using FCQB.Data;
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
    public class QBCustomerQueryRqModel
    {
        //eClatModel db;
        public string Xml { get; set; }
        [XmlElementAttribute("QBXMLMsgsRq", IsNullable = false)]
        public QBCustQrXMLMsgsRq QBMsgsRq;
        //public QBXML QBXmlObject { get; set; }
        public QBCustomerQueryRqModel()
        {

        }
        public string WriteCustomerQueryXml(string jobId, string fromDate, int MaxCount, string iterator, string iteratorID, string fromName, string toName)
        {
            //db = new eClatModel();            
            
            string strQXML = string.Empty;
            CustomerQueryRq Queryrequest = new CustomerQueryRq();
            Queryrequest.requestID = jobId;
            if (MaxCount > 0)
            {
                Queryrequest.MaxReturned = MaxCount.ToString();
                if (iterator != string.Empty)
                {
                    Queryrequest.iterator = iterator;
                }
                if (iteratorID != string.Empty)
                {
                    Queryrequest.iteratorID = iteratorID;
                }

            }
            
            
            //Queryrequest.iterator = "Start";
            Queryrequest.FromModifiedDate = fromDate; // "2016-02-01T16:21:30";

            if (fromName != string.Empty)
            {
                NameRangeFilter filter = new NameRangeFilter();
                filter.FromName = fromName;
                filter.ToName = toName;
                Queryrequest.NameRangeFilter = filter;
            }

            QBCustQrXMLMsgsRq qbMsgReqs = new QBCustQrXMLMsgsRq();
            qbMsgReqs.CustQueryRq = Queryrequest;

            // Create an instance of the XmlSerializer class;
            // specify the type of object to serialize.
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlSerializer serializer =
            new XmlSerializer(typeof(QBCustomerQueryRqModel));

            QBCustomerQueryRqModel modl = new QBCustomerQueryRqModel();
            modl.QBMsgsRq = qbMsgReqs;
            //StringWriter writer1 = new StringWriter();  //@"c:\temp\qbXml.xml"
            // Serialize the model, and close the TextWriter.
            using (StringWriter writer = new Utf8StringWriter())
            {
                serializer.Serialize(writer, modl, ns);
                strQXML = writer.ToString();
            }

            return strQXML;
        }
        public class QBCustQrXMLMsgsRq
        {
            [XmlElementAttribute("CustomerQueryRq", IsNullable = false)]
            public CustomerQueryRq CustQueryRq;
            [XmlAttribute(AttributeName = "onError")]
            public string onError { get; set; }
            public QBCustQrXMLMsgsRq()
            {
                onError = "stopOnError"; ;
            }
        }
        public class CustomerQueryRq
        {
            //[XmlAttribute(AttributeName = "MaxReturned")]
            //public int MaxReturned { get; set; }
            [XmlAttribute(AttributeName = "requestID")]
            public string requestID { get; set; }
            [XmlAttribute(AttributeName = "iterator")]
            
            public string iterator { get; set; }
            [XmlAttribute(AttributeName = "iteratorID")]
            public string iteratorID { get; set; }
            [XmlElementAttribute("MaxReturned")]
            public string MaxReturned { get; set; }   //Laolu changed from int to string . string elements are suppresed in the xml when empty i.e whqn not set
            [XmlElementAttribute("FromModifiedDate")]
            public string FromModifiedDate { get; set; }
            [XmlElementAttribute("NameRangeFilter",IsNullable =false)]
            public NameRangeFilter NameRangeFilter { get; set; }


            public CustomerQueryRq()
            {

            }

            
        }
        public class NameRangeFilter
        {
            [XmlElementAttribute("FromName", Order = 1)]
            public string FromName { get; set; }
            [XmlElementAttribute("ToName", Order = 2)]
            public string ToName { get; set; }
        }
    }
}