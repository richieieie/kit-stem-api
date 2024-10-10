using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Services;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IVNPayService _vnPayService;
        public PaymentsController(IPaymentService paymentService, IVNPayService vnPayService)
        {
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
            var serviceResponse = await _vnPayService.PaymentExecute(Request.Query);
            if (!serviceResponse.Succeeded)
            {
                return StatusCode(serviceResponse.StatusCode, new { status = serviceResponse.Status, details = serviceResponse.Details });
            }

            return Ok(new { status = serviceResponse.Status, details = serviceResponse.Details });
        }
    }
}