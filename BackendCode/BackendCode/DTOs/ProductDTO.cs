namespace BackendCode.DTOs.ProductDTO
{
    public class ProductDTO
    {
        public string ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public bool SaleOrNot { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public string? StoreTag { get; set; }
        public string? ProductPic { get; set; }  // Base64 encoded string or null
    }

    public class PicDes
    {
        public string? Description { get; set; }
        public string DetailPic { get; set; }
        
    }
    public class Product1DTO
    {
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public string? StoreTag { get; set; }
        public List<string>? ProductImages { get; set; }
        public List<PicDes>? PicDes { get; set; }
    }
    public class Product2DTO
    {
        public string ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public string? StoreTag { get; set; }
        public string? ProductPic { get; set; }  // Base64 encoded string or null
    }
}

