using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("BUYER")]
    public class BUYER:ACCOUNT
    {
        //public string ACCOUNT_ID { get; set; }
        public string GENDER { get; set; }
        public int? AGE { get; set; } // AGE 可以为空
        public int TOTAL_CREDITS { get; set; }
        public string ADDRESS { get; set; }
        //public virtual ACCOUNT ACCOUNT { get; set; }
    }
}

