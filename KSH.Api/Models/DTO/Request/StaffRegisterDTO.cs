using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO.Request
{
    public class StaffRegisterDTO : UserRegisterDTO
    {
        [Required(ErrorMessage = "Vui lòng cung cấp tên nhân viên!", AllowEmptyStrings = false)]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp họ của nhân viên!", AllowEmptyStrings = false)]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp số điện thoại của nhân viên!")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải có đúng 10 chữ số.")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp địa chỉ của nhân viên!", AllowEmptyStrings = false)]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Vui lòng cung cấp giới tính của nhân viên!")]
        [Range(0, 2, ErrorMessage = "Giới tính không hợp lệ!")]
        public int GenderCode { get; set; }
        [NotDefaultDate(ErrorMessage = "Vui lòng cung cấp ngày sinh của nhân viên!")]
        public DateTimeOffset BirthDate { get; set; }
    }
}