﻿using System.ComponentModel.DataAnnotations;

namespace KST.Api.Models.DTO
{
    public class ComponentTypeCreateDTO
    {
        [StringLength(100)]
        public string Name { get; set; } = null!;
    }
}
