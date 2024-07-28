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
}

