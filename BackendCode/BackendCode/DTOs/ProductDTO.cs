﻿namespace BackendCode.DTOs.ProductDTO
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
        //public string? ProductPic { get; set; }  // Base64 encoded string or null
        public List<string> ProductPics { get; set; }//新增首页图
        public List<PicDes> ProductDes { get; set; }//新增详情图文
    }

    public class PicDes
    {
        public string? Description { get; set; }
        public IFormFile DetailPic { get; set; }
        
    }

    public class ShowProductDTO
    {
        public string ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public bool SaleOrNot { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public string? StoreTag { get; set; }
        //public string? ProductPic { get; set; }  // Base64 encoded string or null
        public List<ImageModel> ProductPics { get; set; }//新增首页图
        public List<ShowPicDes> ProductDes { get; set; }//新增详情图文
    }
    public class ShowPicDes
    {
        public string? Description { get; set; }
        public ImageModel DetailPic { get; set; }

    }
    public class Product1DTO
    {
        public string? ProductName { get; set; }
        public decimal ProductPrice { get; set; }
        public string? Tag { get; set; }
        public string? Description { get; set; }
        public string? StoreTag { get; set; }
        public List<IFormFile>? ProductImages { get; set; }
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
        //public string? ProductPic { get; set; }  // Base64 encoded string or null
/*        public List<string> ProductPics { get; set; }//新增首页图
        public List<PicDes> ProductDes { get; set; }//新增详情图文*/
    }
}

