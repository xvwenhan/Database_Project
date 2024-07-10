using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class MARKET_PRODUCT
    {
        /* 市集ID */
        public string MARKET_ID { get; set; }

        /* 商品ID */
        public string PRODUCT_ID { get; set; }

        /* 折扣价格 */
        public float? DISCOUNT_PRICE { get; set; }

        public virtual MARKET MARKET { get; set; }
        public virtual PRODUCT PRODUCT { get; set; }
    }
}
