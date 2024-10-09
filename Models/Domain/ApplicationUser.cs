using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace kit_stem_api.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(45)]
        public string? FirstName { get; set; }
        [MaxLength(45)]
        public string? LastName { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        private int points = 0;
        public int Points
        {
            get
            {
                return points;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Minimum point is 0!");
                }
                else
                {
                    points = value;
                }
            }
        }
        public bool Status { get; set; }
        [JsonIgnore]
        [InverseProperty("User")]
        public virtual ICollection<UserOrders> UserOrders { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("Staff")]
        public virtual ICollection<LabSupport> LabSupports { get; set; } = null!;

        [JsonIgnore]
        [InverseProperty("User")]
        public virtual ICollection<Cart> Carts { get; set; } = null!;
    }
}