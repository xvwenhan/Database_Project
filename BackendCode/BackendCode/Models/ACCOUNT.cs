using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class ACCOUNT
    {
        [Key]
        [Column("ACCOUNT_ID")]
        public string? ACCOUNT_ID { get; set; }

        [Column("USER_NAME")]
        public string? USERNAME { get; set; }

        [Column("PHONE_NUMBER")]
        public string? PHONE_NUMBER { get; set; }

        [Column("PASSWORD")]
        public string? PASSWORD { get; set; }

    }
}


