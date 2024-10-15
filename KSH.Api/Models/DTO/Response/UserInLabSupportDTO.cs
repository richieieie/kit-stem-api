namespace KSH.Api.Models.DTO.Response
{
    public class UserInLabSupportDTO
    {
        public Guid UserId { get; set; }
        private string? userName = null;
        public string UserName
        {
            get
            {
                return userName!;
            }
            set
            {
                userName = value.Split('@')[0];
            }
        }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
    }
}
