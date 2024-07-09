using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System;

namespace BackendCode.Models
{
    public class ORDERS
    {
        [Key]
        [Column("ORDER_ID")]
        [Required]
        [MaxLength(100)]
        public string ORDER_ID { get; set; }

        [Column("PRODUCT_ID")]
        [MaxLength(100)]
        public string PRODUCT_ID { get; set; }

        [Column("TOTAL_PAY")]
        [Range(0, 99999.99)]
        public decimal? TOTAL_PAY { get; set; }

        [Column("ACTUAL_PAY")]
        [Range(0, 99999.99)]
        public decimal? ACTUAL_PAY { get; set; }

        [Column("ORDER_STATUS")]
        [MaxLength(40)]
        public string ORDER_STATUS { get; set; }

        [Column("CREATE_TIME")]
        public DateTime? CREATE_TIME { get; set; }

        [Column("RECIEVING_TIME")]
        public DateTime? RECIEVING_TIME { get; set; }

        [Column("DELIVERY_NUMBER")]
        [MaxLength(100)]
        public string DELIVERY_NUMBER { get; set; }

        [Column("SCORE")]
        [Range(0, 9.9)]
        public decimal? SCORE { get; set; } = 0;

        [Column("REMARK")]
        [MaxLength(200)]
        public string REMARK { get; set; }

        [Column("BONUS_CREDITS")]
        [Range(0, 9999)]
        public int? BONUS_CREDITS { get; set; }

        [Column("RETURN_OR_NOT")]
        public bool? RETURN_OR_NOT { get; set; }

        [Column("BUYER_ACCOUNT_ID")]
        [MaxLength(100)]
        public string BUYER_ACCOUNT_ID { get; set; }

        [Column("STORE_ACCOUNT_ID")]
        [MaxLength(100)]
        public string STORE_ACCOUNT_ID { get; set; }

        // 导航属性
        [ForeignKey("PRODUCT_ID")]
        public PRODUCT Product { get; set; }

        [ForeignKey("BUYER_ACCOUNT_ID")]
        public BUYER Buyer { get; set; }

        [ForeignKey("STORE_ACCOUNT_ID")]
        public STORE Store { get; set; }
    }

}
