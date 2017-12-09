using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    public class CustItemRef
    {
        [XmlElementAttribute("FullName")]
        public string FullName;
    }
    public class LineItemRef
    {
        [XmlElementAttribute("ListID")]
        public string ListID;
        [XmlElementAttribute("FullName")]
        public string FullName;
    }
}