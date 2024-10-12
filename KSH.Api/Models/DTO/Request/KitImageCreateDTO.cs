using Microsoft.EntityFrameworkCore;

namespace KSH.Api.Models.DTO.Request
{
    public class KitImageCreateDTO
    {
        public Guid Id { get; set; }

        public int KitId { get; set; }

        [Unicode(false)]
        public string Url { get; set; }
    }
}
