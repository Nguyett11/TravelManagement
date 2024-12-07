using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Reporting.Models
{
    [Table("orders_reports")]
    public class orders_reports
    {
        [Key]
        public int id { get; set; }
        public int order_id { get; set; }
        public decimal total_revenue { get; set; }
        public decimal total_cost { get; set;}
        public decimal total_profit { get; set;}
    }
}
