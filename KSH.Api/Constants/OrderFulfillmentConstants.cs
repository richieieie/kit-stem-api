namespace KSH.Api.Constants
{
    public static class OrderFulfillmentConstants
    {
        public const bool PaymentSuccess = true;
        public const bool PaymentFail = false;
        public const string OrderVerifyingStatus = "CHỜ XÁC NHẬN";
        public const string OrderVerifiedStatus = "ĐÃ XÁC NHẬN";
        public const string OrderDeliveringStatus = "ĐANG GIAO HÀNG";
        public const string OrderSuccessStatus = "GIAO HÀNG THÀNH CÔNG";
        public const string OrderFailStatus = "GIAO HÀNG THẤT BẠI";
        public const int PaymentCash = 1;
        public const int PaymentVnPay = 2;
    }
}