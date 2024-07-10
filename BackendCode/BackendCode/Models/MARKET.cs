using Microsoft.Extensions.Hosting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCode.Models
{
    public class MARKET
    {
        /* 市集ID */
        public string MARKET_ID { get; set; }

        /* 主题 */
        public string? THEME { get; set; }

        /* 开始时间 */
        public DateTime? START_TIME { get; set; }

        /* 结束时间 */
        public DateTime? END_TIME { get; set; }

        /* 折扣要求 */
        public string? DISCOUNT_REQUIREMENT { get; set; }
    }
}
