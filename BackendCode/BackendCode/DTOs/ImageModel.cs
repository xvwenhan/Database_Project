namespace BackendCode.DTOs
{
    public class ImageModel
    {
        public string? ImageId { get; set; }
        // 添加一个只读属性来生成图片的URL
        public string ImageUrl
        {
            get
            {
                //return $"https://localhost:7262/api/images/product/{ImageId}";
                return $"http://47.97.5.21:5173/api/images/product/{ImageId}";
            }
        }
    }

    public class PostImageModel
    {
        public string? ImageId { get; set; }
        // 添加一个只读属性来生成图片的URL
        public string ImageUrl
        {
            get
            {
                //return $"https://localhost:7262/api/images/post/{ImageId}";
                return $"http://47.97.5.21:5173/api/images/post/{ImageId}";
            }
        }
    }

    public class MarketImageModel
    {
        public string? ImageId { get; set; }
        // 添加一个只读属性来生成图片的URL
        public string ImageUrl
        {
            get
            {
                //return $"https://localhost:7262/api/images/market/{ImageId}";
                return $"http://47.97.5.21:5173/api/images/market/{ImageId}";
            }
        }
    }
}
