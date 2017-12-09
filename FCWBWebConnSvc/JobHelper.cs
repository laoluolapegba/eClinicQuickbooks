using FCQB.Data;
using FCQBWebConnAPI.Services;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FCQBWebConnAPI
{
    public class JobHelper
    {
        eClatModel db;
        private static readonly ILog log = LogManager.GetLogger(typeof(JobHelper));
        public JobHelper()
        {
            db = new eClatModel();
        }
        public void addJob(JobType jobtype, string patientId)
        {
            try
            {
                qb_jobs jobentity = new qb_jobs();
                jobentity.error_code = "0";
                //jobentity.FromDate = currentResponse.FromDate;
                jobentity.job_type = (int)jobtype;
                jobentity.MaxReturn = 0;
                jobentity.trans_id = patientId;
                jobentity.job_status = "P";
                string new_qb_ticket_id = DateTime.Now.ToString("ddMMyyyyhhmmssffff");
                jobentity.qb_ticket_id = new_qb_ticket_id;
                jobentity.retry_count = 0;
                jobentity.iteratorid = "";
                jobentity.iterator = "";
                jobentity.ext_sys_id = "-";
                jobentity.ext_sys_token = "-";
                db.qb_jobs.Add(jobentity);
                db.SaveChanges();
                log.Info("logged new job with new ticket id :" + new_qb_ticket_id);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}