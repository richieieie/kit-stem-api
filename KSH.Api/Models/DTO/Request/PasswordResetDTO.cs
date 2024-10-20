using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class PasswordResetDTO
    {
        [Required(ErrorMessage = "Email không được để trống")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Token đặt lại mật khẩu là bắt buộc.")]
        public string? Token { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc.")]
        public string? NewPassword { get; set; }
    }
}