﻿using kit_stem_api.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO.Response
{
    public class KitResponseDTO
    {
        public int Id { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string Brief { get; set; } = null!;

        public string Description { get; set; } = null!;

        public int PurchaseCost { get; set; }

        public bool Status { get; set; }

        public virtual KitsCategory? KitsCategory { get; set; }
    }
}