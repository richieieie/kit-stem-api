using AutoMapper;
using KST.Api.Constants;
using KST.Api.Models.Domain;
using KST.Api.Models.DTO.Request;
using KST.Api.Models.DTO.Response;
using KST.Api.Repositories;
using KST.Api.Services.IServices;
using KST.Api.Utils;

namespace KST.Api.Services
{
    public class VNPayService : IVNPayService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public VNPayService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, UnitOfWork unitOfWork, IMapper mapper)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResponse> CreatePaymentUrl(PaymentVnPayCreateDTO paymentVnPayCreateDTO)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(paymentVnPayCreateDTO.OrderId);
                if (order == null)
                {
                    return serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Tạo mới payment thất bại!")
                            .AddError("notFound", "Không tìm thấy order của bạn!");
                }

                var payment = new Payment()
                {
                    Id = Guid.NewGuid(),
                    MethodId = OrderFulfillmentConstants.PaymentVnPay,
                    CreatedAt = DateTimeOffset.Now,
                    Status = OrderFulfillmentConstants.PaymentFail,
                    Amount = order.TotalPrice,
                    OrderId = order.Id
                };

                await _unitOfWork.PaymentRepository.CreateAsync(payment);

                var vnp_ReturnUrl = _configuration["VNPay:vnp_ReturnUrl"]!; //URL nhan ket qua tra ve 
                var vnp_Url = _configuration["VNPay:vnp_Url"]!; //URL thanh toan cua VNPAY 
                var vnp_TmnCode = _configuration["VNPay:vnp_TmnCode"]!; //Ma website
                var vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"]!; //Chuoi bi mat
                if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
                {
                    return serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status500InternalServerError)
                            .AddDetail("message", "Lấy url giao dịch thất bại!")
                            .AddError("outOfService", "Không thể lấy được url giao dịch ngay lúc này, vui lòng liên hệ với nhà cung cấp để được hỗ trợ!");
                }
                var locale = _configuration["VNPay:vnp_Locale"]!;

                //Build URL for VNPAY
                VnPayLibrary vnPay = new VnPayLibrary();
                vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnPay.AddRequestData("vnp_Command", "pay");
                vnPay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
                vnPay.AddRequestData("vnp_Amount", (payment.Amount * 100).ToString());
                vnPay.AddRequestData("vnp_CreateDate", payment.CreatedAt.ToString("yyyyMMddHHmmss"));
                vnPay.AddRequestData("vnp_CurrCode", _configuration["VNPay:vnp_CurrCode"]!);
                vnPay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress(_httpContextAccessor)!);
                if (!string.IsNullOrEmpty(locale))
                {
                    vnPay.AddRequestData("vnp_Locale", locale);
                }
                else
                {
                    vnPay.AddRequestData("vnp_Locale", "vn");
                }
                vnPay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang: {payment.Id}");
                vnPay.AddRequestData("vnp_OrderType", "250000");
                vnPay.AddRequestData("vnp_ReturnUrl", vnp_ReturnUrl);
                vnPay.AddRequestData("vnp_TxnRef", $"{payment.Id}");
                vnPay.AddRequestData("vnp_ExpireDate", payment.CreatedAt.AddMinutes(3).ToString("yyyyMMddHHmmss"));

                string paymentUrl = vnPay.CreateRequestUrl(vnp_Url, vnp_HashSecret);

                return serviceResponse
                            .AddDetail("message", "Lấy url giao dịch thành công!")
                            .AddDetail("data", new { url = paymentUrl });
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới payment thất bại!")
                        .AddError("outOfService", "Không thể tạo một order mới ngay lúc này!");
            }
        }

        public async Task<ServiceResponse> PaymentExecute(IQueryCollection vnPayData)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                if (vnPayData.Count == 0)
                {
                    return serviceResponse
                                .SetSucceeded(false)
                                .SetStatusCode(StatusCodes.Status404NotFound)
                                .AddDetail("message", "Thực hiện giao dịch thất bại!")
                                .AddError("invalidCredentials", "Thông tin giao dịch cung cấp không chính xác, vui lòng kiểm tra lại!");
                }

                string vnp_HashSecret = _configuration["VNPay:vnp_HashSecret"]!;
                var vnPay = new VnPayLibrary();
                foreach (var (key, value) in vnPayData)
                {
                    if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                    {
                        vnPay.AddResponseData(key, value);
                    }
                }

                var paymentId = Guid.Parse(vnPay.GetResponseData("vnp_TxnRef"));
                var vnp_Amount = Convert.ToInt64(vnPay.GetResponseData("vnp_Amount")) / 100;
                var vnPayTranId = Convert.ToInt64(vnPay.GetResponseData("vnp_TransactionNo"));
                var vnp_ResponseCode = vnPay.GetResponseData("vnp_ResponseCode");
                var vnp_TransactionStatus = vnPay.GetResponseData("vnp_TransactionStatus");
                string? vnp_SecureHash = vnPayData.FirstOrDefault(d => d.Key == "vnp_SecureHash").Value;
                var checkSignature = vnPay.ValidateSignature(vnp_SecureHash, vnp_HashSecret);
                if (!checkSignature)
                {
                    return serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Thực hiện giao dịch thất bại!")
                            .AddError("invalidCredentials", "Thông tin giao dịch cung cấp không chính xác, vui lòng kiểm tra lại!");
                }
                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
                if (payment == null || payment.Amount != vnp_Amount)
                {
                    return serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Thực hiện giao dịch thất bại!")
                            .AddError("invalidCredentials", "Thông tin giao dịch cung cấp không chính xác, vui lòng kiểm tra lại!");
                }
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(payment.OrderId);
                if (vnp_ResponseCode != "00" || vnp_TransactionStatus != "00")
                {
                    return serviceResponse
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status500InternalServerError)
                        .AddDetail("message", "Giao dịch thất bại!");
                }

                // Update payment status
                payment.Status = true;
                await _unitOfWork.PaymentRepository.UpdateAsync(payment);
                // Update order status
                order!.ShippingStatus = OrderFulfillmentConstants.OrderVerifyingStatus;
                await _unitOfWork.OrderRepository.UpdateAsync(order);

                return serviceResponse
                        .AddDetail("message", "Giao dịch thành công!");
            }
            catch
            {
                return serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới payment thất bại!")
                        .AddError("outOfService", "Không thể tạo một payment mới ngay lúc này!");
            }
        }
    }
}