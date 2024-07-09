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

        [Required]
        [Column("PRODUCT_NAME")]
        [MaxLength(50)]
        public string PRODUCT_NAME { get; set; }

        [Required]
        [Column("PRODUCT_PRICE")]
        [Range(0, 99999.99)]
        public decimal PRODUCT_PRICE { get; set; }

        [Required]
        [Column("SALE_OR_NOT")]
        public bool SALE_OR_NOT { get; set; }

        [Required]
        [Column("TAG")]
        [MaxLength(50)]
        public string TAG { get; set; }

        [Required]
        [Column("DESCRIBTION")]
        [MaxLength(200)]
        public string DESCRIBTION { get; set; }

        [Required]
        [Column("ACCOUNT_ID")]
        [MaxLength(100)]
        public string ACCOUNT_ID { get; set; }

        // 导航属性
        [ForeignKey("ACCOUNT_ID")]
        public STORE Store { get; set; }
    }
}
