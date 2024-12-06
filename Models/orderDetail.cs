using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderManagement.Models
{
    [Table("OrderDetail")]
    public class orderDetail
    {
        [Key]
        public int id { get; set; }
        public int order_id { get; set; }
        public int product_id { get; set; }
        public string product_name { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal total_price { get; set; }

    }
}
