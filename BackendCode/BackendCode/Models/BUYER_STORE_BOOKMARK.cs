using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("BUYER_STORE_BOOKMARK")]
    public class BUYER_STORE_BOOKMARK
    {
        [Key, Column(Order = 0)]
        [ForeignKey("BUYER")]
        [StringLength(100)]
        public string BUYER_ACCOUNT_ID { get; set; }

        [Key, Column(Order = 1)]
        [ForeignKey("STORE")]
        [StringLength(100)]
        public string STORE_ACCOUNT_ID { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual STORE STORE { get; set; }
    }
}
