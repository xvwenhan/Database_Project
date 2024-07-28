namespace BackendCode.DTOs
{
    public class PostModel
    {
        public string PostTitle { get; set; }
        public string? PostContent { get; set; }
        public List<IFormFile> ?Images { get; set; }

    }

    public class LikePostModel
    {
        public string PostId { get; set; }
    }

    public class ReportPostModel
    {
        public string PostId { get; set; }
        public string ReportReason { get; set; }
    }

}
