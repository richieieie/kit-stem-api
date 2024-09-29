using kit_stem_api.Models.Domain;

namespace kit_stem_api.Models.DTO.Response
{
    public class LabResponseDTO
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Url { get; set; }
        public int Price { get; set; }
        public int MaxSupportTimes { get; set; }
        public string? Author { get; set; }
        public bool Status { get; set; }
        public virtual Kit? Kit { get; set; }
        public virtual Level? Level { get; set; }
    }
}