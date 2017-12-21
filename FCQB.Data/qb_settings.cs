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

    }
}
