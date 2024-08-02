namespace BackendCode.DTOs.Payment
{
    public class ReturnRequestDTO
    {
        public string orderID { get; set; }
        public string ReturnReason { get; set; }
    }

    public class WalletBalanceDTO
    {
        public string AccountId { get; set; }
        public decimal Balance { get; set; }
    }

    public class OrderPaymentDTO
    {
        public string BuyerId { get; set; }
        public string ProductId { get; set; }
        public string StoreId { get; set; }
        public decimal ActualPay { get; set; }
    }

    public class RechargeDTO
    {
        public string BuyerId { get; set; }
        public decimal Amount { get; set; }
    }

    public class RechargedWalletDTO
    {
        public string BuyerId { get; set; }
        public decimal Balance { get; set; }
    }
}