//有关管理员的DTO
namespace BackendCode.DTOs.Administrator
{
    //管理员查看所有商家认证申请
    public class ShowAuthenticationDTO
    {
        public string? StoreId { get; set; }
        public string? Authentication { get; set; }
        public string? Status { get; set; }

    }

    //
    public class FavouriteStoresDTO
    {
        public string? BuyerId { get; set; }
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public decimal? StoreScore { get; set; }

    }
}
