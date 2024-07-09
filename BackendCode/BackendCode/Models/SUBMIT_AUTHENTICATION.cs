using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("SUBMIT_AUTHENTICATION")]
    public class SUBMIT_AUTHENTICATION
    {
        [Key]
        [Column("STORE_ACCOUNT_ID")]
        public string? STORE_ACCOUNT_ID { get; set; }

        [Key]
        [Column("ADMINISTRATOR_ACCOUNT_ID")]
        public string? ADMINISTRATOR_ACCOUNT_ID { get; set; }

        [Required]
        [Column("AUTHENTICATION")]
        public string? AUTHENTICATION { get; set; }

        [ForeignKey("STORE_ACCOUNT_ID")]
        public STORE STORE { get; set; }

        [ForeignKey("ADMINISTRATOR_ACCOUNT_ID")]
        public ADMINISTRATOR ADMINISTRATOR { get; set; }
    }
}
