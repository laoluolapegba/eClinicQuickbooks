using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCQB.Data
{
    [Table("qb_customers")]
    public partial class qb_customers
    {
        [Key]
        public int recordid { get; set; }
        public string upi { get; set; }
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        public string EditSequence { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string IsActive { get; set; }
        public string Sublevel { get; set; }
        public string Balance { get; set; }
        public string TotalBalance { get; set; }
        public string JobStatus { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string LastName { get; set; }

    }
}
