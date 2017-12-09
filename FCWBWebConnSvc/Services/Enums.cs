using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FCQBWebConnAPI.Services
{
    public enum SyncStatus
    {
        NotSynced = 0,
        Synced = 1,
        Failed = 2
    }
    public enum JobType
    {
        CustomerAdd = 1,
        InvoiceAdd = 2,
        PaymentAdd = 3,
        CustomerQuery = 4,
        BillAdd = 5
    }
}