using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("ACCOUNT")] //ACCOUNT表
    public class ACCOUNT
    {
        [Key]
        [Column("ACCOUNT_ID")]
        [StringLength(100)]
        public string ACCOUNT_ID { get; set; }


        [MaxLength(20)]
        [Column("USER_NAME")]
        public string? USERNAME { get; set; }

        [Column("PHONE_NUMBER")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Phone number must be 11 digits long.")]//限制为11位长度
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be 11 digits long and contain only numbers.")]//限制只包含数字
        public string? PHONE_NUMBER { get; set; }

        [MaxLength(15)]
        [Column("PASSWORD")]
        public string? PASSWORD { get; set; }

    }
}


