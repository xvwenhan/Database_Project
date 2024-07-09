using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("ADMINISTRATOR")]
    public class ADMINISTRATOR
    {
        [Key]
        [Column("ACCOUNT_ID")]
        [StringLength(100)]
        public string ACCOUNT_ID { get; set; }

        [ForeignKey("ACCOUNT_ID")]
        public virtual ACCOUNT ACCOUNT { get; set; }

        [Column("PERMISSION_LEVEL")]
        public int PERMISSION_LEVEL { get; set; }
    }
}

