using KSH.Api.Models.DTO.Response;
using KSH.Api.Utils.Interfaces;

namespace KSH.Api.Utils
{
    public class EmailTemplateProvider : IEmailTemplateProvider
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EmailTemplateProvider(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string GetOrderConfirmationTemplate(string shopName, OrderResponseDTO orderDTO)
        {
            string body = string.Empty;
            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Assets", "Templates", "OrderConfirmation.html");

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("[UserName]", orderDTO.User!.UserName);
            body = body.Replace("[OrderId]", orderDTO.Id.ToString());
            body = body.Replace("[CreatedAt]", TimeConverter.ToVietNamTime(orderDTO.CreatedAt).ToString());
            body = body.Replace("[TotalPrice]", orderDTO.TotalPrice.ToString());
            body = body.Replace("[ShippingAddress]", orderDTO.ShippingAddress!.ToString());
            body = body.Replace("[PhoneNumber]", orderDTO.PhoneNumber!.ToString());
            body = body.Replace("[ShippingStatus]", orderDTO.ShippingStatus!.ToString());
            body = body.Replace("[ShopName]", shopName);

            return body;
        }

        public string GetRegisterTemplate(string userName, string shopName, string verifyUrl)
        {
            string body = string.Empty;
            string path = Path.Combine(_webHostEnvironment.ContentRootPath, "Assets", "Templates", "Register.html");

            using (StreamReader reader = new StreamReader(path))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("[UserName]", userName);
            body = body.Replace("[ShopName]", shopName);
            body = body.Replace("[VerifyUrl]", verifyUrl);

            return body;
        }
    }
}