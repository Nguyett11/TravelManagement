using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reporting.Models
{
    [Table("product_reports")]
    public class product_reports
    {
        [Key]
        public int id { get; set; }
        public int order_report_id { get; set; }
        public int product_id { get; set; }
        public int total_sold { get; set; }
        public decimal revenue { get; set; }
        public decimal cost { get; set; }
        public decimal profit { get; set; }
    }
}
