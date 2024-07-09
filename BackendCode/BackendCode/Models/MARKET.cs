using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class MARKET
    {
        /* 市集ID */
        [Key]
        [Column("MARKET_ID")]
        public string MARKET_ID { get; set; }

        /* 主题 */
        [Column("THEME")]
        public string? THEME { get; set; }

        /* 开始时间 */
        [Column("START_TIME")]
        public DateTime? START_TIME { get; set; }

        /* 结束时间 */
        [Column("END_TIME")]
        public DateTime? END_TIME { get; set; }

        /* 折扣要求 */
        [Column("DISCOUNT_REQUIREMENT")]
        public string? DISCOUNT_REQUIREMENT { get; set; }
    }
}
