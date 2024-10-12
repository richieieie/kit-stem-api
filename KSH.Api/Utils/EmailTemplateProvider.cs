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