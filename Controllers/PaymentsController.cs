using kit_stem_api.Models.DTO.Request;
using kit_stem_api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace kit_stem_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly IVNPayService _vnPayService;
        public PaymentsController(IVNPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpGet]
        [Route("VnPay")]
        public async Task<IActionResult> GetAsync()
        {
            // Change vnpPayment below into code that parse a cart from user in to a VNPaymentRequestDTO
            var vnpPayment = new VNPaymentRequestDTO()
            {
                Amount = 144444,
                Description = "Thanh toan thong qua VNPay"
            };

            var serviceResponse = await _vnPayService.CreatePaymentUrl(vnpPayment);
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