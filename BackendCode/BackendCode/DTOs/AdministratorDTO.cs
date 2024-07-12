//有关管理员的DTO
namespace BackendCode.DTOs.Administrator
{
    //管理员查看所有商家认证申请
    public class ShowAuthenticationDTO
    {
        public string? StoreId { get; set; }
        public string? Authentication { get; set; }
        public bool? Status { get; set; }

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
