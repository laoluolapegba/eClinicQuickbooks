using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI
{
    public class ResponseObject
    {
        public int statusCode { get; set; }
        public string statusDesc { get; set; }
        public string statusXML { get; set; }
        public ResponseObject()
        {
            statusCode = 0;
            statusDesc = "";
            statusXML = "";
        }
    }
    [XmlRootAttribute("RootNode", IsNullable = false)]
    public class RootNode
    {
        [XmlElementAttribute("RspObj", IsNullable = false)]
        public RspObj rspObj;
    }
    public class RspObj
    {

        [XmlAttribute(AttributeName = "requestID")]
        public string requestID { get; set; }
        [XmlAttribute(AttributeName = "statusCode")]
        public string statusCode { get; set; }
        [XmlAttribute(AttributeName = "statusSeverity")]
        public string statusSeverity { get; set; }
    }
}