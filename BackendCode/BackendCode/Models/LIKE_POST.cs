using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class LIKE_POST
    {
        /* 买家ID */
        public string BUYER_ACCOUNT_ID { get; set; }

        /* 帖子ID */
        public string POST_ID { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual POST POST { get; set; }
    }
}
