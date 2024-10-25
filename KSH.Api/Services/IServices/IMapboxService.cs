namespace KSH.Api.Services.IServices
{
    public interface IMapboxService
    {
        Task<ServiceResponse> GetDistanceBetweenAnAddressAndShop(string address);
        Task<double?> GetDistanceBetweenAddressAndShop(string address);
    }
}