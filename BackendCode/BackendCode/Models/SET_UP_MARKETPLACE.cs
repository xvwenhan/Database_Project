using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("SET_UP_MARKETPLACE")]
    public class SET_UP_MARKETPLACE
    {
        [Key]
        [Column("MARKET_ID")]
        public string? MARKET_ID { get; set; }

        [Key]
        [Column("ADMINISTRATOR_ACCOUNT_ID")]
        public string? ADMINISTRATOR_ACCOUNT_ID { get; set; }

        [ForeignKey("MARKET_ID")]
        public MARKET MARKET { get; set; }

        [ForeignKey("ADMINISTRATOR_ACCOUNT_ID")]
        public ADMINISTRATOR ADMINISTRATOR { get; set; }

    }
}
