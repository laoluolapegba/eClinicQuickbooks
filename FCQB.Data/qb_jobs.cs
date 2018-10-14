using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCQB.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("qb_jobs")]
    public partial class qb_jobs
    {
        [Key]
        public long jobid { get; set; }
        public string ext_sys_token { get; set; }
        public int job_type { get; set; }
        [StringLength(45)]
        public string trans_id { get; set; }
        [StringLength(1)]
        public string job_status { get; set; }
        [StringLength(45)]
        public string error_code { get; set; }
        [StringLength(255)]
        public string error_description { get; set; }
        public long retry_count { get; set; }
        public string qb_ticket_id { get; set; }        
        public DateTime? ext_sync_date { get; set; }
        public string ext_sys_id { get; set; }
        public DateTime? qb_timemodified { get; set; }
        public string FromDate { get; set; }
        public int MaxReturn { get; set; }
        public string iteratorid { get; set; }
        public string iterator { get; set; }
        public string fromname { get; set; }
        public string toname { get; set; }
        public DateTime? created_date { get; set; }
    }
}
