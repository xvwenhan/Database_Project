using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("RETURN")]
    public class RETURN
    {
        [Key]
        [Column("ORDER_ID")]
        public string? ORDER_ID { get; set; }

        [Required]
        [Column("RETURN_TIME")]
        public DateTime RETURN_TIME { get; set; }

        [Required]
        [Column("RETURN_REASON")]
        public string? RETURN_REASON { get; set; }

        [Required]
        [Column("RETURN_STATUS")]
        public string? RETURN_STATUS { get; set; }

        [ForeignKey("ORDER_ID")]
        public ORDERS ORDERS { get; set; }
    }
}
