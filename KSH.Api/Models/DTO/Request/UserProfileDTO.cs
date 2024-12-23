﻿using System.ComponentModel.DataAnnotations;

namespace KSH.Api.Models.DTO
{
    public class UserProfileDTO
    {
        [Required]
        public string? UserName { get; set; }
        [MaxLength(45)]
        public string? FirstName { get; set; }
        [MaxLength(45)]
        public string? LastName { get; set; }
        [MaxLength(100)]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int GenderCode { get; set; }
        public string? Gender { get; set; }
        public DateTimeOffset BirthDate { get; set; }
        private long points = 0;
        public long Points
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
    }
}
