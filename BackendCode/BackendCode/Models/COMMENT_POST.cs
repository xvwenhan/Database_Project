namespace BackendCode.Models
{
    public class COMMENT_POST
    {
        public string BUYER_ACCOUNT_ID { get; set; }
        public string POST_ID { get; set; }
        public DateTime EVALUATION_TIME { get; set; }
        public string? EVALUATION_COMTENT { get; set; }

        public virtual BUYER BUYER { get; set; }
        public virtual POST POST { get; set; }
    }
}