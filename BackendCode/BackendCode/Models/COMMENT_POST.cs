using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class COMMENT_POST
    {
        /* 买家ID */
        public string BUYER_ACCOUNT_ID { get; set; }

        /* 帖子ID */
        public string POST_ID { get; set; }

        /* 评论时间 */
        public DateTime EVALUATION_TIME { get; set; }

        /* 评论内容 */
        public string? EVALUATION_COMTENT { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual POST POST { get; set; }
    }
}