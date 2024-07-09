using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class MARKET_PRODUCT
    {
        /* 市集ID */
        [Key]
        [ForeignKey("MARKET")]
        [Column("MARKET_ID")]
        public string MARKET_ID { get; set; }

        /* 商品ID */
        [Key]
        [ForeignKey("PRODUCT")]
        [Column("PRODUCT_ID")]
        public string PRODUCT_ID { get; set; }

        /* 折扣价格 */
        [Column("DISCOUNT_PRICE")]
        public float? DISCOUNT_PRICE { get; set; }

        public virtual MARKET MARKET { get; set; }
        public virtual PRODUCT PRODUCT { get; set; }
    }
}
