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
        public string ethnicity { get; set; }

        [StringLength(50)]
        public string religion { get; set; }

        [StringLength(100)]
        public string occupation { get; set; }

        [StringLength(50)]
        public string education { get; set; }

        [StringLength(100)]
        public string government { get; set; }

        [StringLength(50)]
        public string phone { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(50)]
        public string alcohol { get; set; }

        [StringLength(50)]
        public string smoking { get; set; }

        [StringLength(16777215)]
        public string past_medical_conditions { get; set; }

        [StringLength(16777215)]
        public string drug_allergy { get; set; }

        [StringLength(16777215)]
        public string family_history { get; set; }

        [StringLength(16777215)]
        public string past_surgeries { get; set; }

        [StringLength(16777215)]
        public string cardiac_history { get; set; }

        [Column(TypeName = "date")]
        public DateTime? last_menstral_period { get; set; }

        [StringLength(20)]
        public string menstral_duration { get; set; }

        [StringLength(20)]
        public string menstral_regularity { get; set; }

        [StringLength(20)]
        public string menstral_flow { get; set; }

        [StringLength(10)]
        public string menstral_pain { get; set; }

        public sbyte? gravidity { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string medications { get; set; }

        [Column(TypeName = "tinytext")]
        [StringLength(255)]
        public string notes { get; set; }

        [StringLength(100)]
        public string marital_status { get; set; }

        public short? origin_lga { get; set; }

        public short? origin_state { get; set; }

        [StringLength(255)]
        public string origin_country { get; set; }

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

        public int? assigned_doctor { get; set; }

        public int? main_consultant { get; set; }

        public double? credit_limit { get; set; }

        public double? discount_percent { get; set; }

        [StringLength(50)]
        public string bloodgroup { get; set; }

        [StringLength(50)]
        public string genotype { get; set; }

        [StringLength(100)]
        public string nok_full_name { get; set; }

        [StringLength(50)]
        public string nok_relationship { get; set; }

        [StringLength(50)]
        public string nok_phone { get; set; }

        [Column(TypeName = "tinytext")]
        [StringLength(255)]
        public string nok_address { get; set; }

        public int created_by { get; set; }

        public DateTime created_at { get; set; }

        public int? modified_by { get; set; }

        public DateTime? modified_at { get; set; }

        public bool inactive { get; set; }

        public bool? local_syncflag { get; set; }

        public DateTime? local_sync_at { get; set; }

        public bool? online_syncflag { get; set; }

        public DateTime? online_sync_at { get; set; }

        [StringLength(5)]
        public string immunization_llin { get; set; }

        [StringLength(5)]
        public string immunization_deworming { get; set; }

        [StringLength(5)]
        public string immunization_vitamin_A { get; set; }

        public bool? immunization_status { get; set; }

        public bool? is_mortal { get; set; }

        public DateTime? mortality_date { get; set; }

        [StringLength(100)]
        public string mortality_type { get; set; }

        [Column(TypeName = "text")]
        [StringLength(65535)]
        public string mortality_reason { get; set; }

        public bool sync_flag { get; set; }

        public DateTime? sync_date { get; set; }

        [StringLength(50)]
        public string parity { get; set; }

        [StringLength(50)]
        public string postcoital_bleeding { get; set; }

        [StringLength(50)]
        public string menarche_age { get; set; }

        [StringLength(50)]
        public string menopause_age { get; set; }

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
