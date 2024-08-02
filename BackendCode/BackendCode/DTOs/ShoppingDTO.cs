namespace BackendCode.DTOs.Shopping
{
    public class StoreInfoDTO
    {
        public string name { get; set; }
        public decimal? score { get; set; }
    }

    public class AuthenticationInfoDTO
    {
        public string storeID { get; set; }
        public List<IFormFile>? image { get; set; }
        public string description { get; set; }
    }

    public class SetStoreInfoDTO
    {
        public string storeID { get; set; }
        public string address { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

    public class GetStoreInfoDTO
    {
        public string storeID { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
    }
}