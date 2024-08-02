using Microsoft.Extensions.Hosting;

namespace BackendCode.Models
{
    public class BUYER : ACCOUNT
    {
        public string? GENDER { get; set; }
        public int? AGE { get; set; } // AGE 可以为空
        public int TOTAL_CREDITS { get; set; }
        public string? ADDRESS { get; set; }

        //实验/////////////////////////////////////
        public virtual ICollection<POST> POSTS { get; } = new List<POST>();
        //public virtual ICollection<POST> POSTS { get; set; } 
        public virtual ICollection<COMMENT_POST> COMMENT_POSTS { get; } = new List<COMMENT_POST>();
        public virtual ICollection<COMMENT_COMMENT> COMMENT_COMMENTS { get; set; }
        public virtual ICollection<COMPLAIN_POST> COMPLAIN_POSTS { get; set; }
        public virtual ICollection<COMPLAIN_COMMENT> COMPLAIN_COMMENTS { get; set; }
    }
}

