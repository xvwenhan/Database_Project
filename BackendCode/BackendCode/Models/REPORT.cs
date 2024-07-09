using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendCode.Models
{
    public class REPORT
    {
        [Key]
        [Column("REPORT_ID")]
        [Required]
        [MaxLength(100)]
        public string REPORT_ID { get; set; }

        [Column("REPORTING_TIME")]
        public DateTime? REPORTING_TIME { get; set; }

        [Column("REPORTING_REASON")]
        [MaxLength(200)]
        public string REPORTING_REASON { get; set; }

        [Column("AUDIT_TIME")]
        public DateTime? AUDIT_TIME { get; set; }

        [Column("AUDIT_RESULTS")]
        [MaxLength(10)]
        public string AUDIT_RESULTS { get; set; }

        [Column("ADMINISTRATOR_ACCOUNT_ID")]
        [MaxLength(100)]
        public string ADMINISTRATOR_ACCOUNT_ID { get; set; }

        // 导航属性
        [ForeignKey("ADMINISTRATOR_ACCOUNT_ID")]
        public ADMINISTRATOR Administrator { get; set; }
    }
}
