namespace KSH.Api.Models.DTO.Response
{
    public class UserStaffInLabSupportDTO
    {
        // public string? UserName { get; set; }    
        private string? userName = null;
        public string? UserName
        {
            get
            {
                return userName!;
            }
            set
            {
                if (value != null)
                {
                    userName = value.Split('@')[0];
                }
                else
                {
                    userName = null;
                }
            }
        }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
    }
}
