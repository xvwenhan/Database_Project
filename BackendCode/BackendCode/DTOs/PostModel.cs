using BackendCode.DTOs.CommentModel;
namespace BackendCode.DTOs.PostModel
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

    public class GetPostsModel
    {
        public string SortBy { get; set; } = "time";
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
    public class PostDetailModel
    {
        public string PostId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public int? NumberOfLikes { get; set; }
        public int? NumberOfComments { get; set; }
        public List<ImageModel>? Images { get; set; }
        public List<CommentDetailModel> Comments { get; set; }
    }

    public class ImageModel
    {
        public string ImageId { get; set; }
        public byte[] Image { get; set; }
    }



}
