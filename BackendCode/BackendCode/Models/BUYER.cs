namespace BackendCode.Models
{
    public class BUYER:ACCOUNT
    {
        public string GENDER { get; set; }
        public int? AGE { get; set; } // AGE 可以为空
        public int TOTAL_CREDITS { get; set; }
        public string ADDRESS { get; set; }
    }
}

