using KSH.Api.Models.DTO.Response;

namespace KSH.Api.Utils.Interfaces
{
    public interface IEmailTemplateProvider
    {
        string GetPasswordResetTemplate(string username, string shopName, string passwordResetUrl);
        string GetOrderConfirmationTemplate(string shopName, OrderResponseDTO orderDTO);
        string GetRegisterTemplate(string userName, string shopName, string verifyUrl);
    }
}