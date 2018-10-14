namespace FCQB.Data
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("patients")]
    public partial class patient
    {
        [Key]
        public string patient_id { get; set; }

        [StringLength(50)]
        public string clinic_token { get; set; }

        public int clinic_id { get; set; }

        [StringLength(100)]
        public string upi { get; set; }

        [StringLength(100)]
        public string card_number { get; set; }

        [StringLength(10)]
        public string currency { get; set; }

        [StringLength(20)]
        public string title { get; set; }

        [StringLength(100)]
        public string middle_name { get; set; }

        [StringLength(100)]
        public string forename { get; set; }

        [Required]
        [StringLength(100)]
        public string surname { get; set; }

        [StringLength(10)]
        public string gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dob { get; set; }

        [Column(TypeName = "tinytext")]
        [StringLength(255)]
        public string address { get; set; }
        
        [StringLength(50)]
        public string phone { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string hmo { get; set; }

        [StringLength(100)]
        public string hmo_number { get; set; }

        [StringLength(255)]
        public string hmo_company { get; set; }

        [StringLength(100)]
        public string hmo_authorization_number { get; set; }

        public int? hmo_policy_id { get; set; }

        public DateTime? hmo_valid_from { get; set; }

        public DateTime? hmo_valid_to { get; set; }

        [StringLength(100)]
        public string photo { get; set; }

        [StringLength(255)]
        public string patient_type { get; set; }

        [StringLength(255)]
        public string patient_department { get; set; }

        public double? credit_limit { get; set; }

        public double? discount_percent { get; set; }

        public int created_by { get; set; }

        public DateTime created_at { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_at { get; set; }

        public bool inactive { get; set; }

        public bool? local_syncflag { get; set; }

        public DateTime? local_sync_at { get; set; }

        public bool? online_syncflag { get; set; }

        public DateTime? online_sync_at { get; set; }

        public bool sync_flag { get; set; }

        public DateTime? sync_date { get; set; }

        [StringLength(255)]
        public string assigned_user_id { get; set; }

        public short? invoiced_by { get; set; }

        public DateTime? invoiced_at { get; set; }

        public int? locked_by { get; set; }

        public DateTime? locked_at { get; set; }
        public string ext_sys_id { get; set; }
        public bool ext_sync_flag { get; set; }
        public DateTime? ext_sync_date { get; set; }
    }
}
