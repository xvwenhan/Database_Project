using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("SET_UP_MARKETPLACE")]
    public class SET_UP_MARKETPLACE
    {
        public string? MARKET_ID { get; set; }

        public string? ADMINISTRATOR_ACCOUNT_ID { get; set; }

        public virtual MARKET MARKET { get; set; }

        public virtual ADMINISTRATOR ADMINISTRATOR { get; set; }
    }
}
