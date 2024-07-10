using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    [Table("ADMINISTRATOR")]
    public class ADMINISTRATOR:ACCOUNT
    {
        //public string ACCOUNT_ID { get; set; }
        public int PERMISSION_LEVEL { get; set; }
        //public virtual ACCOUNT ACCOUNT { get; set; }
    }
}

