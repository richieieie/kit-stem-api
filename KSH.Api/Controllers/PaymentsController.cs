using KSH.Api.Models.DTO.Request;
using KSH.Api.Services;
using KSH.Api.Services.IServices;
using KSH.Api.Utils.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KSH.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        private readonly IEmailService _emailService;
        private readonly IPaymentService _paymentService;
        private readonly IVNPayService _vnPayService;
        public PaymentsController(IEmailTemplateProvider emailTemplateProvider, IEmailService emailService, IPaymentService paymentService, IVNPayService vnPayService)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailService = emailService;
            _paymentService = paymentService;
            _vnPayService = vnPayService;
        }

        [HttpPost]
        [Route("Cash")]
        public async Task<IActionResult> CreateCashAsync([FromBody] PaymentCreateDTO paymentCreateDTO)
        {
            var serviceResponse = await _paymentService.CreateCashAsync(paymentCreateDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpPost]
        [Route("VnPay")]
        public async Task<IActionResult> CreateVnPayAsync([FromBody] PaymentVnPayCreateDTO paymentVnPayCreateDTO)
        {
            var serviceResponse = await _vnPayService.CreatePaymentUrl(paymentVnPayCreateDTO);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }

        [HttpGet]
        [Route("VnPay/Callback")]
        public async Task<IActionResult> GetVnPayCallbackAsync()
        {
            var (serviceResponse, orderDTO) = await _vnPayService.PaymentExecute(Request.Query);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            var subject = "Chào mừng bạn đến với shop!";
            var body = _emailTemplateProvider.GetOrderConfirmationTemplate("KitStemHub", orderDTO);
            await _emailService.SendEmail(orderDTO.User.UserName, subject, body);

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}