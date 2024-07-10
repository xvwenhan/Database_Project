using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendCode.Models
{
    public class REPORT
    {
        public string REPORT_ID { get; set; }

        public DateTime? REPORTING_TIME { get; set; }

        public string REPORTING_REASON { get; set; }


        public DateTime? AUDIT_TIME { get; set; }


        public string AUDIT_RESULTS { get; set; }


        public string ADMINISTRATOR_ACCOUNT_ID { get; set; }

        // 导航属性
        public virtual ADMINISTRATOR Administrator { get; set; }
    }
}
