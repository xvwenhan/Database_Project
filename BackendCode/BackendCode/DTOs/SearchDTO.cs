using System.Reflection.Metadata;

namespace BackendCode.DTOs.Search
{
    //返回搜到的商品的DTO
    public class SearchProductsDTO
    {
        public string? ProductName { get; set; }
        public string? StoreId { get; set; }
        public string? ProductId { get; set; }
        public decimal? ProductPrice { get; set; }
        public bool? SaleOrNot { get; set; }
        public string? Tag { get; set; }
        //public byte[]? Pic { get; set; }

    }

    //返回搜到店铺的DTO
    public class SearchStoresDTO
    {
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public decimal? StoreScore { get; set; }

    }
}
