namespace BackendCode.Models
{
    public class COMMENT_COMMENT
    {
        public string COMMENT_ID { get; set; }
        public string ACCOUNT_ID { get; set; }
        public string COMMENTED_COMMENT_ID { get; set; }
        public DateTime COMMENT_TIME { get; set; }
        public string? COMMENT_COMTENT { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual POST COMMENT_POST { get; set; }
    }
}
