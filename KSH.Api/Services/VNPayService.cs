using AutoMapper;
using KSH.Api.Constants;
using KSH.Api.Models.Domain;
using KSH.Api.Models.DTO.Request;
using KSH.Api.Models.DTO.Response;
using KSH.Api.Repositories;
using KSH.Api.Services.IServices;
using KSH.Api.Utils;


namespace KSH.Api.Services
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
                    CreatedAt = TimeConverter.GetCurrentVietNamTime(),
                    Status = OrderFulfillmentConstants.PaymentFail,
                    Amount = order.TotalPrice,
                    OrderId = order.Id
                };
                await _unitOfWork.PaymentRepository.CreateAsync(payment);

                //Build URL for VNPAY
                var vnPay = new VnPayLibrary();
                vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
                vnPay.AddRequestData("vnp_Command", "pay");
                vnPay.AddRequestData("vnp_TmnCode", _configuration["VNPay:vnp_TmnCode"]!);
                vnPay.AddRequestData("vnp_Amount", (payment.Amount * 100).ToString());
                vnPay.AddRequestData("vnp_CreateDate", payment.CreatedAt.ToString("yyyyMMddHHmmss"));
                vnPay.AddRequestData("vnp_CurrCode", _configuration["VNPay:vnp_CurrCode"]!);
                vnPay.AddRequestData("vnp_IpAddr", Utils.Utils.GetIpAddress(_httpContextAccessor)!);
                vnPay.AddRequestData("vnp_Locale", _configuration.GetValue("VNPay:vnp_Locale", "vn") ?? "vn");
                vnPay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang: {payment.Id}");
                vnPay.AddRequestData("vnp_OrderType", "250000");
                vnPay.AddRequestData("vnp_ReturnUrl", _configuration["VNPay:vnp_ReturnUrl"]!);
                vnPay.AddRequestData("vnp_TxnRef", $"{payment.Id}");
                vnPay.AddRequestData("vnp_ExpireDate", payment.CreatedAt.AddMinutes(_configuration.GetValue("VNPay:vpn_TransactionTimeOut", 5)).ToString("yyyyMMddHHmmss"));

                string paymentUrl = vnPay.CreateRequestUrl(_configuration["VNPay:vnp_Url"]!, _configuration["VNPay:vnp_HashSecret"]!);

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

        public async Task<(ServiceResponse, OrderResponseDTO?)> PaymentExecute(IQueryCollection vnPayData)
        {
            var serviceResponse = new ServiceResponse();
            try
            {
                if (vnPayData.Count == 0)
                {
                    return (serviceResponse
                                .SetSucceeded(false)
                                .SetStatusCode(StatusCodes.Status404NotFound)
                                .AddDetail("message", "Thực hiện giao dịch thất bại!")
                                .AddError("invalidCredentials", "Thông tin giao dịch cung cấp không chính xác, vui lòng kiểm tra lại!"), null);
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
                    return (serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Thực hiện giao dịch thất bại!")
                            .AddError("invalidCredentials", "Thông tin giao dịch cung cấp không chính xác, vui lòng kiểm tra lại!"), null);
                }
                var payment = await _unitOfWork.PaymentRepository.GetByIdAsync(paymentId);
                if (payment == null || payment.Amount != vnp_Amount)
                {
                    return (serviceResponse
                            .SetSucceeded(false)
                            .SetStatusCode(StatusCodes.Status404NotFound)
                            .AddDetail("message", "Thực hiện giao dịch thất bại!")
                            .AddError("invalidCredentials", "Thông tin giao dịch cung cấp không chính xác, vui lòng kiểm tra lại!"), null);
                }
                var order = await _unitOfWork.OrderRepository.GetByIdAsync(payment.OrderId);
                if (vnp_ResponseCode != "00" || vnp_TransactionStatus != "00")
                {
                    return (serviceResponse
                        .SetSucceeded(false)
                        .SetStatusCode(StatusCodes.Status500InternalServerError)
                        .AddDetail("message", "Giao dịch thất bại!"), null);
                }

                // Update payment status
                payment.Status = true;
                await _unitOfWork.PaymentRepository.UpdateAsync(payment);
                // Update order status
                order!.ShippingStatus = OrderFulfillmentConstants.OrderVerifyingStatus;
                await _unitOfWork.OrderRepository.UpdateAsync(order);
                await _unitOfWork._dbContext.Entry(order).Reference(o => o.User).LoadAsync();

                var orderDTO = _mapper.Map<OrderResponseDTO>(order);

                return (serviceResponse
                        .AddDetail("message", "Giao dịch thành công!"), orderDTO);
            }
            catch
            {
                return (serviceResponse
                        .SetSucceeded(false)
                        .AddDetail("message", "Tạo mới payment thất bại!")
                        .AddError("outOfService", "Không thể tạo một payment mới ngay lúc này!"), null);
            }
        }

        private DateTimeOffset GetCurrentVietNamTime()
        {
            var createdAtUtc = DateTimeOffset.UtcNow;

            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var createdAtInVietnam = TimeZoneInfo.ConvertTime(createdAtUtc, vietnamTimeZone);

            return createdAtInVietnam;
        }
    }
}