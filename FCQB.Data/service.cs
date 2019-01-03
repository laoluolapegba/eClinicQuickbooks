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

    [Table("services")]
    public partial class service
    {
        [Key]
        public int id { get; set; }
        public string clinic_token { get; set; }
        public int parent_id { get; set; }
        public string service_token { get; set; }
        public string service_code { get; set; }
        public string service_name { get; set; }
        public int service_type { get; set; }
        public int is_capitation { get; set; }
        public int is_variable { get; set; }
        public double amount { get; set; }
        public string notes { get; set; }
        public string summary_grouping { get; set; }
        public int created_by { get; set; }
        public DateTime? created_at { get; set; }
        public int? sync_flag { get; set; }
        public DateTime? sync_date { get; set; }
        public int sync_id { get; set; }
        public int sync_mode { get; set; }
        public int inactive { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_at { get; set; }
        public string default_report_by { get; set; }
        public int last_saved_by { get; set; }
        public DateTime? last_saved_at { get; set; }
        public string sales_account { get; set; }
        public string cogs_account { get; set; }
        public string inventory_account { get; set; }
        public string adjustment_account { get; set; }
        public string assembly_account { get; set; }

    }
}
