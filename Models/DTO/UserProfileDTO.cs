using System.ComponentModel.DataAnnotations;

namespace kit_stem_api.Models.DTO
{
    public class UserProfileDTO
    {
        public string UserName { get; set; }
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
    }
}
