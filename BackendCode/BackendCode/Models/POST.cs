namespace BackendCode.Models
{
    public class POST
    {
        public string POST_ID { get; set; }
        public DateTime? RELEASE_TIME { get; set; }
        public string? POST_CONTENT { get; set; }
        public int? NUMBER_OF_LIKES { get; set; }
        public int? NUMBER_OF_COMMENTS { get; set; }
        public string ACCOUNT_ID { get; set; }

        public string POST_TITLE { get; set; }

        public virtual BUYER BUYER { get; set; }
    }
}
