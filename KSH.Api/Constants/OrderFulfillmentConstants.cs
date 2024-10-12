namespace KSH.Api.Constants
{
    public static class OrderFulfillmentConstants
    {
        public const bool PaymentSuccess = true;
        public const bool PaymentFail = false;
        public const string OrderVerifyingStatus = "VERIFYING";
        public const string OrderDeliveringStatus = "DELIVERING";
        public const string OrderSuccessStatus = "SUCCESS";
        public const string OrderFailStatus = "FAIL";
        public const int PaymentCash = 1;
        public const int PaymentVnPay = 2;
    }
}