using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendCode.Models
{
    public class MARKET_STORE
    {
        [Key]
        [MaxLength(100)]
        [Column("MARKET_ID")]
        public string MARKET_ID { get; set; }

        [MaxLength(100)]
        [Column("STORE_ACCOUNT_ID")]
        public string STORE_ACCOUNT_ID { get; set; }


        [Column("IN_OR_NOT ")]
        public bool IN_OR_NOT { get; set; }
    }
}
