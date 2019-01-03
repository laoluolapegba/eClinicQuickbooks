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

    [Table("qb_items")]
    public partial class qb_item
    {
        [Key]
        
        public int recordid { get; set; }
        public string ItemId { get; set; }
        [Required]
        public string ListID { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeModified { get; set; }
        public string EditSequence { get; set; }
        public string Name { get; set; }
        public string IsActive { get; set; }
        public string Sublevel { get; set; }
        public string Balance { get; set; }
        public string TotalBalance { get; set; }
        public string JobStatus { get; set; }


        //      recordid` int(11) NOT NULL,
        //`itemid` varchar(255) DEFAULT NULL,
        //`listid` varchar(255) DEFAULT NULL,
        //`name` varchar(255) DEFAULT NULL,
        //`timecreated` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
        //`timemodified` datetime DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,
        //`editsequence` varchar(255) DEFAULT NULL,
        //`isactive` varchar(20) DEFAULT NULL,
        //`sublevel` varchar(255) DEFAULT NULL,
        //`balance` varchar(50) DEFAULT NULL,
        //`totalbalance` varchar(50) DEFAULT NULL,
        //`jobstatus` varchar(50) DEFAULT NULL
    }
}
