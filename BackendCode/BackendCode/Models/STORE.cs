﻿namespace BackendCode.Models
{
    public class STORE : ACCOUNT
    {
        public string? ACCOUNT_ID { get; set; }
        public string? STORE_NAME { get; set; }
        public decimal STORE_SCORE { get; set; }
        public string? CERTIFICATION { get; set; }
        public string? ADDRESS { get; set; }
    }
}
