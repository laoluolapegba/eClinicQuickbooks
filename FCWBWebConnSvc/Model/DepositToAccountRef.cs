using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    public class DepositToAccountRef
    {
        [XmlElementAttribute("ListID", Order = 1)]
        public string ListID { get; set; }
        [XmlElementAttribute("FullName", Order = 2)]
        public string FullName { get; set; }
    }
}