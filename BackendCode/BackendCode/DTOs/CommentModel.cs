namespace BackendCode.DTOs
{
    public class CommentModel
    {
        public string PostId;
        public string CommentContext;
    }

    public class CommentModel2
    {
        public string CommentId;
        public string CommentContext;
    }
    public class CommentDeleteModel
    {
        public string CommentId;
    }

    public class ReportCommentModel {
        public string CommentId { get; set; }
        public string ReportReason { get; set; }
    }

    public class CommentDetailModel
    {
        public string CommentId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime CommentTime { get; set; }
        public string? CommentContent { get; set; }
        public List<SubCommentDetailModel> SubComments { get; set; }  //子评论列表
    }

    public class SubCommentDetailModel
    {
        public string CommentId { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string CommentedCommentId { get; set; }
        public DateTime CommentTime { get; set; }
        public string? CommentContent { get; set; }
    }

}

