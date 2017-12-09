namespace FCQB.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cust_allocations")]
    public partial class cust_allocations
    {

        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string clinic_token { get; set; }

        [Required]
        [StringLength(50)]
        public string allocation_token { get; set; }

        [Column(TypeName = "date")]
        public DateTime date_alloc { get; set; }

        [StringLength(100)]
        public string trans_no_from { get; set; }

        public int? trans_type_from { get; set; }

        [StringLength(100)]
        public string trans_no_to { get; set; }

        public int? trans_type_to { get; set; }

        public int? created_by { get; set; }

        public DateTime? created_at { get; set; }
        public DateTime? sync_date { get; set; }
        public bool sync_flag { get; set; }
        public string ext_sys_id { get; set; }
        public bool ext_sync_flag { get; set; }
        public DateTime ext_sync_date { get; set; }
    }
}
