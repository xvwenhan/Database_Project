namespace BackendCode.DTOs.Favourite
{
    public class FavouriteProductsDTO
    {
        public string? BuyerId { get; set; }
        public string? StoreId { get; set; }
        public string? ProductId { get; set; }
        public decimal? ProductPrice { get; set; }
        public bool? SaleOrNot { get; set; }
        public string? Tag { get; set; }
        
    }

    public class FavouriteStoresDTO
    {
        public string? BuyerId { get; set; }
        public string? StoreId { get; set; }
        public string? StoreName { get; set; }
        public decimal? StoreScore { get; set; }

    }
}
