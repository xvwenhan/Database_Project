using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class COMPLAIN_POST
    {
        /* 买家ID */
        [ForeignKey("BUYER")]
        [Column("BUYER_ACCOUNT_ID")]
        public string BUYER_ACCOUNT_ID { get; set; }

        /* 帖子ID */
        [ForeignKey("POST")]
        [Column("POST_ID")]
        public string POST_ID { get; set; }

        /* 举报记录ID */
        [ForeignKey("REPORT")]
        [Column("REPORT_ID")]
        public string REPORT_ID { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual POST POST { get; set; }
        public virtual REPORT REPORT { get; set; }
    }
}
