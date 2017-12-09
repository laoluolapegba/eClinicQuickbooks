using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
 
namespace FCQBWebConnAPI.Model
{
    public class OrderedItem1
    { 
        [XmlElementAttribute("ItemRef", Order = 1)]
        public LineItemRef LineItem;
        [XmlElementAttribute("Desc", Order = 2)]
        public string ItemDescription { get; set; }
        [XmlElementAttribute("Quantity", Order = 3)]
        public int Quantity;
        [XmlElementAttribute("Rate", Order = 4)]
        public double Rate;
        
    }
}