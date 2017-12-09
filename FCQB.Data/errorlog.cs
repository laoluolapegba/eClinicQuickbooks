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

    [Table("qb_errors")]
    public partial class errorlog
    {
        [Key]
        public  int id { get; set; }
        public string ticketid { get; set; }
        public string lasterrorcode { get; set; }
        public string lasterrormsg { get; set; }
        public DateTime errordate { get; set; }
    }
}
