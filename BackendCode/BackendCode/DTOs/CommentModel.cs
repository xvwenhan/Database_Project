namespace BackendCode.DTOs
{
    public class CommentModel
    {
        public string PostId;
        public string CommentContext;
    }

    public class CommentDeleteModel
    {
        public string PostId;
        public string CommentPeopleId;
    }
}

