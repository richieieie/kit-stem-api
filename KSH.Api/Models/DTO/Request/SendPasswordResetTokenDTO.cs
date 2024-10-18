using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KSH.Api.Models.DTO.Request
{
    public class SendPasswordResetTokenDTO
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Vui lòng cung cấp email đăng nhập của bạn")]
        public string? Email { get; set; }
    }
}