namespace BackendCode.DTOs
{
    public class PostModel
    {
        public string PostTitle { get; set; }
        public string? PostContent { get; set; }
        public List<IFormFile> Images { get; set; }

    }
}
