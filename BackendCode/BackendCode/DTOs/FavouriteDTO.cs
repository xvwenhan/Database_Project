//有关收藏的DTO
namespace BackendCode.DTOs.Favourite
{
    //返回所有用户收藏商品的DTO
    public class FavouriteProductsDTO
    {
        public string? BuyerId { get; set; }
        public string? StoreId { get; set; }
        public string? ProductId { get; set; }
        public decimal? ProductPrice { get; set; }
        public bool? SaleOrNot { get; set; }
        public string? Tag { get; set; }
        
    }

    //返回所有用户收藏店铺的DTO
    public class FavouriteStoresDTO
    {
        public string? BuyerId { get; set; }
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public decimal? StoreScore { get; set; }

    }
}
