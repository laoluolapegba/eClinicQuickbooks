using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Data.Entity.Validation;
using System.Configuration;

namespace FCQB.Data
{
    public partial class eClatModel : DbContext
    {
        
        public eClatModel()
            : base("name=eClatModel")
        {
            Database.SetInitializer<eClatModel>(null);
        }
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                var newException = new FormattedDbEntityValidationException(e);
                throw newException;
            }
        }
        public virtual DbSet<cust_allocations> cust_allocations { get; set; }
        public virtual DbSet<debtor_trans> debtor_trans { get; set; }
        public virtual DbSet<debtor_trans_details> debtor_trans_details { get; set; }
        public virtual DbSet<patient> patients { get; set; }
        public virtual DbSet<qb_jobs> qb_jobs { get; set; }
        public virtual DbSet<items> items { get; set; }
        public virtual DbSet<errorlog> errorlogs { get; set; }
        public  virtual DbSet<qb_customers> qb_customers { get; set; }
        public virtual DbSet<qb_settings> qb_settings { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schemaName = ConfigurationManager.AppSettings["SchemaName"];
            modelBuilder.HasDefaultSchema(schemaName);
            modelBuilder.Entity<cust_allocations>()
                .Property(e => e.clinic_token)
                .IsUnicode(false);

            modelBuilder.Entity<cust_allocations>()
                .Property(e => e.allocation_token)
                .IsUnicode(false);

            modelBuilder.Entity<cust_allocations>()
                .Property(e => e.trans_no_from)
                .IsUnicode(false);

            modelBuilder.Entity<cust_allocations>()
                .Property(e => e.trans_no_to)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans>()
                .Property(e => e.clinic_token)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans>()
                .Property(e => e.package_token)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans>()
                .Property(e => e.trans_token)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans>()
                .Property(e => e.debtor_no)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans>()
                .Property(e => e.reference)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans>()
                .Property(e => e.void_details)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans_details>()
                .Property(e => e.clinic_token)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans_details>()
                .Property(e => e.detail_token)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans_details>()
                .Property(e => e.debtor_trans_no);

            modelBuilder.Entity<debtor_trans_details>()
                .Property(e => e.payment_option)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans_details>()
                .Property(e => e.payment_details)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans_details>()
                .Property(e => e.service_id)
                .IsUnicode(false);

            modelBuilder.Entity<debtor_trans_details>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.clinic_token)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.upi)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.card_number)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.currency)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.middle_name)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.forename)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.surname)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.gender)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.ethnicity)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.religion)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.occupation)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.education)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.government)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.alcohol)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.smoking)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.past_medical_conditions)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.drug_allergy)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.family_history)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.past_surgeries)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.cardiac_history)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.menstral_duration)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.menstral_regularity)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.menstral_flow)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.menstral_pain)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.medications)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.notes)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.marital_status)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.origin_country)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.hmo)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.hmo_number)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.hmo_company)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.hmo_authorization_number)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.photo)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.patient_type)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.patient_department)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.bloodgroup)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.genotype)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.nok_full_name)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.nok_relationship)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.nok_phone)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.nok_address)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.immunization_llin)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.immunization_deworming)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.immunization_vitamin_A)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.mortality_type)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.mortality_reason)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.parity)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.postcoital_bleeding)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.menarche_age)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.menopause_age)
                .IsUnicode(false);

            modelBuilder.Entity<patient>()
                .Property(e => e.assigned_user_id)
                .IsUnicode(false);
        }
    }
}
