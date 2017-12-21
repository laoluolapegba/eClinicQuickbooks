using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FCQBWebConnAPI
{
    public class MsgSetResponse
    {
        //List<QBMsgResponses> ResponseList;
        [XmlElementAttribute("InvoiceLineRet", Order = 1)]
        public QBMsgResponses[] ResponseList;
        public List<QBMsgResponses> getResponseList(string response)
        {
            throw new NotImplementedException();
        }

        public class QBMsgResponses
        {
            [XmlElementAttribute("TxnLineID", Order = 1)]
            public string tranId { get; set; }
            [XmlElementAttribute("TxnLineID", Order = 2)]
            public string TxnLineID { get; set; }
        }
    }
}