using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model.ItemInventory
{
    [XmlRootAttribute("QBXML", IsNullable = false)]
    public class QBItemAddRsModel
    {
        [XmlElementAttribute("QBXMLMsgsRs", IsNullable = false)]
        public QBXMLMsgsRs QBMsgsRs;
        public QBItemAddRsModel()
        {

        }
        public QBItemAddRsModel ReadItemAddRsXml(string strResponseXml)
        {
            // Create an instance of the XmlSerializer class;
            // specify the type of object to be deserialized.
            XmlSerializer serializer = new XmlSerializer(typeof(QBItemAddRsModel));
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
            QBItemAddRsModel responseObject;
            // Use CustomerAddRsModel Deserialize method to restore the object's state with
            //data from the XML document.
            responseObject = (QBItemAddRsModel)serializer.Deserialize(sr);


            return responseObject;
        }
        public class QBXMLMsgsRs
        {

            [XmlElementAttribute("ItemInventoryAddRs", IsNullable = false)]
            public ItemInventoryAddRs ItemAddRs;
            public QBXMLMsgsRs()
            {

            }
        }

        public class ItemInventoryAddRs
        {
            [XmlElementAttribute("ItemInventoryRet", IsNullable = false)]
            public ItemAddRetModel ItemAdd;
            [XmlAttribute(AttributeName = "statusCode")]
            public int statusCode { get; set; }
            [XmlAttribute(AttributeName = "statusSeverity")]
            public string statusSeverity { get; set; }
            [XmlAttribute(AttributeName = "statusMessage")]
            public string statusMessage { get; set; }
        }

        public class ItemAddRetModel
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

            [XmlElement(ElementName = "Isactive", Order = 7)]
            public bool IsActive { get; set; }           



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