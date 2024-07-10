using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("SUBMIT_AUTHENTICATION")]
    public class SUBMIT_AUTHENTICATION
    {
        public string? STORE_ACCOUNT_ID { get; set; }

        public string? ADMINISTRATOR_ACCOUNT_ID { get; set; }

        public string? AUTHENTICATION { get; set; }

        public STORE STORE { get; set; }

        public virtual ADMINISTRATOR ADMINISTRATOR { get; set; }
    }
}
