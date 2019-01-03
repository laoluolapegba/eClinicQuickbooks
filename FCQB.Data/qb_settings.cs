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

    [Table("qb_settings")]
    public partial class qb_settings
    {
        [Key]
        public  int companyid { get; set; }
        public string itemlistid { get; set; }
        public string itemname { get; set; }
        public string sales_account { get; set; }
        public string cogs_account { get; set; }
        public string inventory_account { get; set; }
        public string adjustment_account { get; set; }
        public string assembly_account { get; set; }
        //sales_account, cogs_account, inventory_account, adjustment_account, assembly_account

    }
}
