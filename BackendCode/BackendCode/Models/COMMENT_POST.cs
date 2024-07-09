using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class COMMENT_POST
    {
        /* 买家ID */
        [Key]
        [ForeignKey("BUYER")]
        [Column("BUYER_ACCOUNT_ID")]
        public string BUYER_ACCOUNT_ID { get; set; }

        /* 帖子ID */
        [Key]
        [ForeignKey("POST")]
        [Column("POST_ID")]
        public string POST_ID { get; set; }

        /* 评论时间 */
        [Key]
        [Column("EVALUATION_TIME")]
        public DateTime EVALUATION_TIME { get; set; }

        /* 评论内容 */
        [Column("EVALUATION_COMTENT")]
        public string? EVALUATION_COMTENT { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual POST POST { get; set; }
    }
}