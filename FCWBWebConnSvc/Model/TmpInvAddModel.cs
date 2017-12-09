using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FCQBWebConnAPI.Model
{
    public class TmpInvAddModel
    {

        public string CustomerNo { get; set; }
        public string TxnDate;
        public string RefNumber { get; set; }         
        public string PONumber { get; set; }
        public string Memo { get; set; }
        public string DebtorNo { get; set; }
    }
    public class TmpPayAddModel
    {

        public string CustomerNo { get; set; }

        public string TxnDate;
        public long RefNumber { get; set; }
        public string PONumber { get; set; }
        public string Memo { get; set; }
        public string DebtorNo { get; set; }
        public double TotalAmount { get; set; }
    }
}