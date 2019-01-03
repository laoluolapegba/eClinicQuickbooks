namespace FCQB.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("debtor_trans_details")]
    public partial class debtor_trans_details
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string clinic_token { get; set; }

        [Required]
        [StringLength(50)]
        public string detail_token { get; set; }
                
        public string debtor_trans_no { get; set; }

        public int? debtor_trans_type { get; set; }

        [StringLength(100)]
        public string payment_option { get; set; }

        [StringLength(255)]
        public string payment_details { get; set; }

        [StringLength(100)]
        public string service_id { get; set; }

        public string is_packaged { get; set; }

        [Column(TypeName = "tinytext")]
        [StringLength(255)]
        public string description { get; set; }

        public double unit_price { get; set; }

        public double unit_tax { get; set; }

        public int quantity { get; set; }

        public double discount_percent { get; set; }

        public double standard_cost { get; set; }

        public double qty_done { get; set; }

        public double? payment { get; set; }

        public bool? allocated { get; set; }

        public int src_id { get; set; }

        public bool? sync_flag { get; set; }

        public long? sync_id { get; set; }

        public bool? sync_mode { get; set; }

        public DateTime? sync_date { get; set; }
        public string bill_item_type { get; set; }
        public string bill_item_id { get; set; }
        public string ext_bill_list_id { get; set; }

        public bool inactive { get; set; }
        public string sales_account { get; set; }
        public string cogs_account { get; set; }
        public string inventory_account { get; set; }
        public string adjustment_account { get; set; }
        public string assembly_account         { get; set; }

        

    }
}
