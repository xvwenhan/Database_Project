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
                //return $"https://localhost:7262/api/images/{ImageId}";
                return $"http://47.97.5.21:5173/api/images/{ImageId}";
            }
        }
    }
}
