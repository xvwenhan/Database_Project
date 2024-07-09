using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("STORE")]
    public class STORE
    {
        [Key]
        [Column("ACCOUNT_ID")]
        public string? ACCOUNT_ID { get; set; }

        [Required]
        [Column("STORE_NAME")]
        public string? STORE_NAME { get; set; }

        [Required]
        [Column("STORE_SCORE")]
        public decimal STORE_SCORE { get; set; }

        [Required]
        [Column("CERTIFICATION")]
        public string? CERTIFICATION { get; set; }

        [Required]
        [Column("ADDRESS")]
        public string? ADDRESS { get; set; }

        [ForeignKey("ACCOUNT_ID")]
        public ACCOUNT ACCOUNT { get; set; }

    }
}
