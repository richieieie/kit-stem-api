﻿using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class ComponentUpdateDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập ID!")]
        public int Id { get; set; }
        public int TypeId { get; set; }

        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
