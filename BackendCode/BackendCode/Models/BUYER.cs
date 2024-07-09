using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("BUYER")]
    public class BUYER
    {
        [Key]
        [Column("ACCOUNT_ID")]
        [StringLength(100)]
        public string ACCOUNT_ID { get; set; }

        [ForeignKey("ACCOUNT_ID")]
        public virtual ACCOUNT ACCOUNT { get; set; }

        [Column("GENDER")]
        [StringLength(10)]
        public string GENDER { get; set; }

        [Column("AGE")]
        public int? AGE { get; set; } // AGE 可以为空

        [Column("TOTAL_CREDITS")]
        [Required]
        public int TOTAL_CREDITS { get; set; }

        [Column("ADDRESS")]
        [StringLength(100)]
        [Required]
        public string ADDRESS { get; set; }
    }
}

