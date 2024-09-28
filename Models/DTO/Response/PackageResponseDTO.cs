using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;

namespace kit_stem_api.Models.DTO.Response
{
    public class PackageResponseDTO
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public bool Status { get; set; }
        public virtual Level? Level { get; set; }
        public virtual Kit? Kit { get; set; }
        public virtual ICollection<LabInsidePackageResponseDTO>? PackageLabs { get; set; }
    }
}