using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("ACCOUNT")] //ACCOUNT表
    public class ACCOUNT
    {
        public string ACCOUNT_ID { get; set; }
        public string? USER_NAME { get; set; }
        public string? PHONE_NUMBER { get; set; }
        public string? PASSWORD { get; set; }

    }
}


