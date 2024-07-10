using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("BUYER_STORE_BOOKMARK")]
    public class BUYER_STORE_BOOKMARK
    {
        public string BUYER_ACCOUNT_ID { get; set; }

        public string STORE_ACCOUNT_ID { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual STORE STORE { get; set; }
    }
}
