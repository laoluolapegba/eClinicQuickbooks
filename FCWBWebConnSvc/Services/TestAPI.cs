using FCQB.Data;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FCQBWebConnAPI.Services
{
    public class TestAPI :ITestAPI
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TestAPI));
        eClatModel db;
        public bool IsWorkAvailable()
        {
        int count = db.debtor_trans.Where(a => a.sync_flag == false).Count();
        return count > 0;
        }
    }
    
}