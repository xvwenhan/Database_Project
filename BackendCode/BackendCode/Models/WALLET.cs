using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("WALLET")]
    public class WALLET
    {
        public string ACCOUNT_ID { get; set; }
        public decimal BALANCE { get; set; }
        public virtual ACCOUNT ACCOUNT { get; set; }
    }
}


