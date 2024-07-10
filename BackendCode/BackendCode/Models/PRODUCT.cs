using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendCode.Models
{
    public class PRODUCT
    {
        [Key]
        [Column("PRODUCT_ID")]
        [Required]
        [MaxLength(100)]
        public string PRODUCT_ID { get; set; }

        public string PRODUCT_NAME { get; set; }


        public decimal PRODUCT_PRICE { get; set; }


        public bool SALE_OR_NOT { get; set; }

        public string TAG { get; set; }

        public string DESCRIBTION { get; set; }


        public string ACCOUNT_ID { get; set; }

        // 导航属性
        public virtual STORE STORE{ get; set; }
    }
}
