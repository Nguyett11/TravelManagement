using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int id { get; set; }
        public string  customer_name { get; set; }
        public string customer_email { get; set; }
        public decimal total_amount { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set;}

    }
}
