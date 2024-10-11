namespace KST.Api.Utils.Interfaces
{
    public interface IEmailTemplateProvider
    {
        string GetRegisterTemplate(string userName, string shopName, string verifyUrl);
    }
}