namespace BackendCode.DTOs.Store
{
    public class MarketDTO
    {
        public string MarketId { get; set; }
        public string? Theme { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Detail { get; set; }
        public string? PosterImg { get; set; } // URL or Base64 string of the image
        public bool IsStoreParticipating { get; set; }
    }

    public class UpdateMarketStoreDTO
    {
        public string MarketId { get; set; }
        public string StoreId { get; set; }
        public bool InOrOut { get; set; }
    }
}
