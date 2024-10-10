using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO
{
    public class UserLoginDTO
    {
        [EmailAddress(ErrorMessage = "Bạn phải đăng nhập bằng email!")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mật khẩu!")]
        public string? Password { get; set; }
    }
}