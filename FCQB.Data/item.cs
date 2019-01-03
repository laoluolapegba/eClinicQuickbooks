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

    [Table("inventory_items")]
    public partial class item
    {
        [Key]
        public int id { get; set; }
        public string sku { get; set; }
        public string actual_stock_item { get; set; }
        public string barcode { get; set; }
        public int category_id { get; set; }
        public string description { get; set; }
        public string long_description { get; set; }
        public string units { get; set; }
        public string uom_conversions { get; set; }
        public string sales_account { get; set; }
        public string cogs_account { get; set; }
        public string inventory_account { get; set; }
        public string adjustment_account { get; set; }
        public string assembly_account { get; set; }
        public double actual_cost { get; set; }
        public double last_cost { get; set; }
        public double material_cost { get; set; }
        public double labour_cost { get; set; }
        public double overhead_cost { get; set; }
        public int reorder_level { get; set; }
        public int max_level { get; set; }
        public int critical_level { get; set; }
        public double markup_percent { get; set; }
        public int inactive { get; set; }
        public DateTime? last_saved_at { get; set; }
        public int?  sync_flag{ get; set; }
        public DateTime? sync_date { get; set; }
        public int sync_id { get; set; }
        public int sync_mode { get; set; }
        public double? volume { get; set; }
        public int last_saved_by { get; set; }
        public string expiry_flag { get; set; }

    }
}
