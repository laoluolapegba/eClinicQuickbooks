namespace FCQB.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("debtor_trans")]
    public partial class debtor_trans
    {
        [Key]
        //[Column(TypeName = "uint")]
        public long trans_no { get; set; }

        [Required]
        [StringLength(50)]
        public string clinic_token { get; set; }

        public int? pos { get; set; }

        [StringLength(255)]
        public string package_token { get; set; }

        [Required]
        [StringLength(50)]
        public string trans_token { get; set; }

        [Column(TypeName = "usmallint")]
        public int type { get; set; }

        public bool version { get; set; }

        [StringLength(50)]
        public string debtor_no { get; set; }

        public int branch_code { get; set; }

        [Column(TypeName = "date")]
        public DateTime tran_date { get; set; }

        public bool? is_packaged { get; set; }

        [Column(TypeName = "date")]
        public DateTime due_date { get; set; }

        [Required]
        [StringLength(60)]
        public string reference { get; set; }

        public int tpe { get; set; }

        public string order_ { get; set; }

        public double ov_amount { get; set; }

        public double ov_gst { get; set; }

        public double ov_freight { get; set; }

        public double ov_freight_tax { get; set; }

        public double ov_discount { get; set; }

        public double alloc { get; set; }

        public double rate { get; set; }

        public int? ship_via { get; set; }

        public int? payment_terms { get; set; }

        public DateTime? created_at { get; set; }

        public int? created_by { get; set; }

        public bool sync_flag { get; set; }

        public long sync_id { get; set; }

        public bool? sync_mode { get; set; }

        public DateTime? sync_date { get; set; }

        [Column(TypeName = "mediumint")]
        public int? voided_by { get; set; }

        public DateTime? voided_at { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string void_details { get; set; }
        public string ext_sys_id { get; set; }
        public bool ext_sync_flag { get; set; }
        public DateTime? ext_sync_date { get; set; }
    }
}
