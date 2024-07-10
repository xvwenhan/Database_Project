using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("RETURN")]
    public class RETURN
    {
        public string? ORDER_ID { get; set; }

        public DateTime RETURN_TIME { get; set; }

        public string? RETURN_REASON { get; set; }

        public string? RETURN_STATUS { get; set; }

        public virtual ORDERS ORDERS { get; set; }
    }
}
