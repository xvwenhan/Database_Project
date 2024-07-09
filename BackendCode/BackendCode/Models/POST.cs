using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace BackendCode.Models
{
    public class POST
    {
        [Key]
        [Column("POST_ID")]
        [StringLength(100)]
        public string POST_ID { get; set; }

        [Column("RELEASE_TIME")]
        public DateTime? RELEASE_TIME { get; set; }

        [Column("POST_CONTENT")]
        [StringLength(200)]
        public string POST_CONTENT { get; set; }

        [Column("NUMBER_OF_LIKES")]
        public int? NUMBER_OF_LIKES { get; set; }

        [Column("NUMBER_OF_COMMENTS")]
        public int? NUMBER_OF_COMMENTS { get; set; }

        [Column("ACCOUNT_ID")]
        [StringLength(100)]
        public string ACCOUNT_ID { get; set; }

        [ForeignKey("ACCOUNT_ID")]
        public virtual BUYER Buyer { get; set; }
    }
}
