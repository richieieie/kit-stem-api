using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kit_stem_api.Models.DTO.Request
{
    public class OrderGetDTO
    {
        public int Page { get; set; }
        public DateTimeOffset CreatedFrom { get; set; } = DateTimeOffset.MinValue;
        public DateTimeOffset CreatedTo { get; set; } = DateTimeOffset.MaxValue;
    }
}