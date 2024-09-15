﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Models.Domain
{
    [Table("Payment")]
    public class Payment
    {
        [Key]
        public Guid Id { get; set; }

        public int? MethodId { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public bool Status { get; set; }

        [ForeignKey("MethodId")]
        [InverseProperty("Payments")]
        public virtual Method Method { get; set; } = null!;

        [InverseProperty("Payment")]
        public virtual ICollection<UserOrders> UserOrders { get; set; } = new List<UserOrders>();
    }
}