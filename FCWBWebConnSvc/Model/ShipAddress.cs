﻿using System.Xml.Serialization;

namespace FCQBWebConnAPI.Model
{
    public class ShipAddress
    {
        [XmlElement(ElementName = "Addr1", Order = 1)]
        public string Addr1 { get; set; }
        [XmlElement(ElementName = "Addr2", Order = 2)]
        public string Addr2 { get; set; }
        [XmlElement(ElementName = "Addr3", Order = 3)]
        public string Addr3 { get; set; }
        [XmlElement(ElementName = "City", Order = 4)]
        public string City { get; set; }
        [XmlElement(ElementName = "State", Order = 5)]
        public string State { get; set; }
        [XmlElement(ElementName = "PostalCode", Order = 6)]
        public string PostalCode { get; set; }
        [XmlElement(ElementName = "Country", Order = 7)]
        public string Country { get; set; }
    }
}