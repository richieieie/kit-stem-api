namespace KSH.Api.Models.DTO.Request
{
    public class UserManagerGetDTO
    {
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool Status { get; set; }
    }
}