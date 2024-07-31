namespace BackendCode.Models
{
    public class PRODUCT
    {
        public string PRODUCT_ID { get; set; }
        public string? PRODUCT_NAME { get; set; }
        public decimal? PRODUCT_PRICE { get; set; }
        public bool? SALE_OR_NOT { get; set; }
        public string? TAG { get; set; }
        public string? DESCRIBTION { get; set; }
        public string? ACCOUNT_ID { get; set; }
        public byte[]? PRODUCT_PIC { get; set; }
        public string? STORE_TAG { get; set; }
        public STORE STORE { get; set; }
    }
}
