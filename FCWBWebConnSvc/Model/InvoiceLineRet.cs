using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    public class InvoiceLineRet
    {
        [XmlElementAttribute("TxnLineID", Order = 1)]
        public string TxnLineID { get; set; }
        [XmlElementAttribute("ItemRef", Order = 2)]
        public LineItemRef LineItem;
        [XmlElementAttribute("Desc", Order = 3)]
        public string ItemDescription { get; set; }
        [XmlElementAttribute("Quantity", Order = 4)]
        public int Quantity;
        [XmlElementAttribute("Rate", Order = 5)]
        public decimal Rate;
        [XmlElementAttribute("Amount", Order = 6)]
        public decimal Amount;
        [XmlElementAttribute("SalesTaxCodeRef", Order = 7)]
        public SalesTaxCodeRef SalesTaxCodeRf { get; set; }

    }
}