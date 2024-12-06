using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TourManagement.Models
{
    [Table("Tour")]
    public class Tour
    {
        [Key]
        public int Id { get; set; }
        public string? Ten { get; set; }
        public int category_id { get; set; }
        public string? NoiXuatPhat { get; set; }
        public string? NoiDen { get; set; }
        public string? ThoiGian { get; set; }
        public int? Gia { get; set; }
        public string? Anh { get; set; }
    }
}

