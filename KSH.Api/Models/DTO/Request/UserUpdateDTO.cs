﻿using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng điền tên của bạn!", AllowEmptyStrings = false)]
        [MaxLength(45)]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền họ của bạn!", AllowEmptyStrings = false)]
        [MaxLength(45)]
        public string? LastName { get; set; }
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
        public string? PhoneNumber { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        [Range(0, 2, ErrorMessage = "Giới tính không hợp lệ!")]
        public int GenderCode { get; set; }
        public DateTimeOffset BirthDate { get; set; }
    }
}
