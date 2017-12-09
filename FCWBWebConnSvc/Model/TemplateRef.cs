using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    public class TemplateRef
    {
        [XmlElementAttribute("ListID", Order = 1)]
        public string ListID { get; set; }
        [XmlElementAttribute("FullName", Order = 2)]
        public string FullName { get; set; }
    }
}