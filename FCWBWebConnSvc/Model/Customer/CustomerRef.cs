using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    public class CustomerRef
    {
        [XmlElementAttribute("ListID", Order = 1)]
        public string ListID;
        //public CustItemRef ListID;
        //[XmlElementAttribute("FullName", Order = 2)]
        //public CustItemRef FullName;
    }
}