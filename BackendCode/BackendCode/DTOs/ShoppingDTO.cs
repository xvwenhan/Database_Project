﻿namespace BackendCode.DTOs.Shopping
{
    public class StoreInfoDTO
    {
        public string name { get; set; }
        public decimal? score { get; set; }
        public string? address { get; set; }
        public byte[]? picture { get; set; }
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

    public class OrderRemarkDTO
    {
        public string orderId { get; set; }
        public string remark { get; set; }
        public decimal score { get; set; }
    }

    public class ProductDetailsDTO
    {
        public string Name { get; set; }
        public List<ImageModel>? Pictures { get; set; } 
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string StoreName { get; set; }
        public string StoreId { get; set; }
        public decimal DiscountPrice { get; set; }
        public string FromWhere { get; set; }
        public decimal? Score { get; set; }
        public bool IsProductStared { get; set; }
        public bool IsStoreStared { get; set; }
        public string? StoreAvatar { get;set; }
        public Dictionary<string, string>? ImageAndText { get; set; }
    }

    public class StoreOrderDTO
    {
        public string OrderId { get; set; }
        public string? BuyerName { get; set; }
        public byte[]? BuyerAvatar { get; set; }
        public decimal OrderScore { get; set; }
        public string? OrderRemark { get; set; }
    }
}