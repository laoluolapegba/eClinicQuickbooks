using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
}