using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("WALLET")]
    public class WALLET
    {
        [Key]
        [Column("ACCOUNT_ID")]
        public string ACCOUNT_ID { get; set; }

        [Required]
        [Column("BALANCE")]
        public decimal BALANCE { get; set; }

        [ForeignKey("ACCOUNT_ID")]
        public ACCOUNT ACCOUNT { get; set; }
    }
}


